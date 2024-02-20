using System.Text;
using ClangSharp;
using ClangSharp.Interop;
using CppToC.Model;
using static ClangSharp.Interop.CXTypeKind;
using static ClangSharp.Interop.CX_TypeClass;

namespace CppToC;


/// <summary>
/// Translates Types into type strings for C.
/// </summary>
public class CTypeTranslator
{
    public readonly List<TemplateArgumentSet> TemplateArgumentStack = new();
    public bool IncludeConstantArraySizes = true;
    public bool IncludeTemplateArgs = true;
    public IReadOnlyList<string> NsPrefixToOmit = Array.Empty<string>();

    public CTypeTranslator() { }

    public CTypeTranslator(CTypeTranslator other)
    {
        TemplateArgumentStack = other.TemplateArgumentStack.ToList();
        IncludeConstantArraySizes = other.IncludeConstantArraySizes;
        IncludeTemplateArgs = other.IncludeTemplateArgs;
        NsPrefixToOmit = other.NsPrefixToOmit;
    }

    public void PushTemplateArgumentSet(TemplateArgumentSet set)
    {
        TemplateArgumentStack.Add(set);
    }

    public void PopTemplateArgumentSet()
    {
        TemplateArgumentStack.RemoveAt(TemplateArgumentStack.Count - 1);
    }
    
    public string GetCType(TypeRef type)
    {
        return GetCType(type.ClangType);
    }

    public IEnumerable<string> GetParameters(FunctionData data)
    {
        return data.Parameters.Select(p => $"{GetCType(p.Type.ClangType)} {p.Name}");
    }
    
    public string GetCType(ClangSharp.Type type)
    {
        switch (type.Kind) {
            case CXType_Invalid:
                break;
            case CXType_Unexposed:
                return GetUnexposedCType(type);
            case CXType_Void:
            case CXType_Bool:
            case CXType_Char_U:
            case CXType_UChar:
            case CXType_Char16:
            case CXType_Char32:
            case CXType_UShort:
            case CXType_UInt:
            case CXType_ULong:
            case CXType_ULongLong:
            case CXType_UInt128:
            case CXType_Char_S:
            case CXType_SChar:
            case CXType_WChar:
            case CXType_Short:
            case CXType_Int:
            case CXType_Long:
            case CXType_LongLong:
            case CXType_Int128:
            case CXType_Float:
            case CXType_Double:
            case CXType_LongDouble:
                break;
            case CXType_NullPtr:
                return "void*";
            case CXType_Overload:
                throw new NotImplementedException();
            case CXType_Dependent:
                throw new NotImplementedException();
            case CXType_ObjCId:
            case CXType_ObjCClass:
            case CXType_ObjCSel:
            case CXType_Float128:
            case CXType_Half:
            case CXType_Float16:
            case CXType_ShortAccum:
            case CXType_Accum:
            case CXType_LongAccum:
            case CXType_UShortAccum:
            case CXType_UAccum:
            case CXType_ULongAccum:
            case CXType_BFloat16:
            case CXType_Ibm128:
            case CXType_Complex:
                return type.AsString;
            case CXType_Pointer: 
            case CXType_LValueReference:
            case CXType_RValueReference: {
                string pointerToken = "*";
                if (type.IsLocalConstQualified) {
                    pointerToken += "const";
                }
                
                if (type.PointeeType.Kind == CXType_FunctionProto) {
                    FunctionProtoType protoType = (FunctionProtoType)type.PointeeType;
                    string[] paramCTypes = protoType.ParamTypes.Select(pt => GetCType(pt)).ToArray();
                    string returnCType = GetCType(protoType.ReturnType);
                    return $"{returnCType} ({pointerToken})({string.Join(',', paramCTypes)})";
                }
                
                string inner = GetCType(type.PointeeType);
                return $"{inner}{pointerToken}";
            }
            case CXType_BlockPointer:
                throw new NotImplementedException();
            case CXType_Record:
            case CXType_Enum: {
                TagType tagType = (TagType)type;
                if (tagType.Decl is ClassTemplateSpecializationDecl ctsDecl) {
                    return GetTemplateSpecializationCType(ctsDecl);
                }
                return GetNamespacedName(CUtil.GetNsFromCursor(tagType.Decl), tagType.Decl.Name);
            }
            case CXType_Typedef:
                return GetTypedefCType((TypedefType)type);
            case CXType_ObjCInterface:
            case CXType_ObjCObjectPointer:
            case CXType_FunctionNoProto:
            case CXType_FunctionProto:
                throw new InvalidOperationException("Should have been handled by Pointer type case.");
            case CXType_ConstantArray: {
                ConstantArrayType arrayType = (ConstantArrayType)type;
                if (IncludeConstantArraySizes) {
                    return $"{GetCType(arrayType.ElementType)}[{arrayType.Size}]";    
                }
                return GetCType(arrayType.ElementType);
            }
            case CXType_Vector:
            case CXType_IncompleteArray:
            case CXType_VariableArray:
            case CXType_DependentSizedArray:
            case CXType_MemberPointer:
            case CXType_Auto:
                throw new NotImplementedException();
            case CXType_Elaborated: {
                ElaboratedType et = (ElaboratedType)type;
                string prefix = "";
                if (type.IsLocalConstQualified) {
                    prefix = "const ";
                }
                return prefix + GetCType(et.NamedType);
            }
            case CXType_Pipe:
                throw new NotImplementedException();
            case CXType_ExtVector:
                throw new NotImplementedException();
            case CXType_Atomic:
                throw new NotImplementedException();
            case CXType_BTFTagAttributed:
                throw new NotImplementedException();
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        return type.AsString;
    }

    public string GetTypedefCType(TypedefType type)
    {
        if (type.AsString == "uintptr_t" || type.AsString == "size_t") {
            return "void*";
        }
        return GetCType(type.Desugar);
    }

    public string GetUnexposedCType(ClangSharp.Type type)
    {
        switch (type.TypeClass) {
            case CX_TypeClass_SubstTemplateTypeParm: {
                SubstTemplateTypeParmType substType = (SubstTemplateTypeParmType)type;
                return GetCType(substType.ReplacementType);
            }
            case CX_TypeClass_TemplateSpecialization: {
                TemplateSpecializationType specializationType = (TemplateSpecializationType)type;
                if (!IncludeTemplateArgs) {
                    return GetNamespacedName(specializationType.TemplateName.AsTemplateDecl);
                }
                return GetTemplateSpecializationCType(specializationType.TemplateName.AsTemplateDecl, specializationType.Args);
            }
            case CX_TypeClass_TemplateTypeParm: {
                TemplateTypeParmType typeParamType = (TemplateTypeParmType)type;

                if (TemplateArgumentStack.Count > 0) {
                    TemplateArgumentSet set = TemplateArgumentStack[(int)typeParamType.Depth];
                    return GetCType(set.Args[(int)typeParamType.Index].AsType);    
                }

                return typeParamType.AsString;
            }
            case CX_TypeClass_InjectedClassName:
                return GetUnexposedCType(((InjectedClassNameType)type).InjectedTST);
            case CX_TypeClass_Using:
                return GetCType(type.Desugar);
            default:
                throw new NotImplementedException();
        }
    }
    
    public string GetTemplateSpecializationCType(ClassTemplateSpecializationDecl decl)
    {
        return GetTemplateSpecializationCType(decl, decl.TemplateArgs);
    }
    
    public string GetTemplateSpecializationCType(NamedDecl templateDecl, IReadOnlyList<TemplateArgument> args)
    {
        return $"{GetNamespacedName(templateDecl)}_{GetTemplateParamCtypePostfix(args)}";
    }
    
    public string GetTemplateParamCtypePostfix(IReadOnlyList<TemplateArgument> args)
    {
        StringBuilder builder = new();
        for (int i = 0, ilen = args.Count; i < ilen; ++i) {
            TemplateArgument arg = args[i];
            if (arg.Kind == CXTemplateArgumentKind.CXTemplateArgumentKind_Integral) {
                builder.Append(arg.AsIntegral);
                continue;
            }

            string cType = GetCType(arg.AsType);
            cType = cType.Replace(" ", "");
            cType = cType.Replace("*", "ptr");
            
            builder.Append(cType);
            if (i + 1 < ilen) {
                builder.Append('_');
            }
        }
        return builder.ToString();
    }

    public bool ShouldPassReturnValueAsOutParameter(ClangSharp.Type type)
    {
        ClangSharp.Type returnClangType = type;
        while (returnClangType.IsSugared) {
            returnClangType = returnClangType.Desugar;
        }
        if (returnClangType is TagType tagType) {
            if (tagType.Declaration is CXXRecordDecl) {
                // Is a record! Gottta pass as an out parameter due to calling convention weirdness.
                return true;
            }
        }

        return false;
    }
    
    public string GetCFunctionLine(FunctionData data, IMethodOwner? selfOf)
    {
        bool passReturnValueAsOutParameter = false;
        
        // Check if our return value needs to be passed as an out parameter.
        if (data.ReturnType != null) {
            passReturnValueAsOutParameter = ShouldPassReturnValueAsOutParameter(data.ReturnType.ClangType);
        }
        
        string returnType = "void";
        if (data.ReturnType != null && !passReturnValueAsOutParameter) {
            returnType = GetCType(data.ReturnType);
        }

        string name = GetCFunctionName(data, selfOf);

        IEnumerable<string> selfPrefix = Array.Empty<string>();
        if (selfOf != null) {
            // TODO: If this is a const function, make the self pointer const.
            selfPrefix = Enumerable.Repeat($"{GetNamespacedName(selfOf)}* __self", 1);
        }

        IEnumerable<string> outParameters = Array.Empty<string>();
        if (passReturnValueAsOutParameter && data.ReturnType != null) {
            outParameters = Enumerable.Repeat($"{GetCType(data.ReturnType)}* pOut", 1);
        }
        
        string parameters = string.Join(", ", selfPrefix.Concat(GetParameters(data).Concat(outParameters)));
        return $"{returnType} {name}({parameters})";
    }
    
    public string GetNamespacedName(string[] ns, string name, IReadOnlyList<TemplateArgument> templateArgs)
    {
        string prefix = GetNamespacedName(ns, name);
        string postfix = GetTemplateParamCtypePostfix(templateArgs);
        if (string.IsNullOrEmpty(postfix)) {
            return prefix;
        }
        return $"{prefix}_{postfix}";
    }

    public string GetNamespacedName(IMethodOwner methodOwner)
    {
        return GetNamespacedName(methodOwner.Namespace, methodOwner.Name, methodOwner.TemplateArguments);
    }
    
    public string GetCFunctionName(FunctionData function, IMethodOwner? selfOf, bool withOverload = true)
    {
        string name;
        if (function.IsOperator) {
            name = "operator_" + CUtil.GetOperatorOverloadName(function.OverloadedOperator);
        } else {
            name = function.Name;
        }

        if (function.OverloadIndex > 0 && withOverload) {
            name += $"{function.OverloadIndex}";
        }
        
        if (selfOf != null) {
            name = $"{GetNamespacedName(selfOf)}_{name}";
        }
        
        name = GetNamespacedName(function.Namespace, name);
        
        return name;
    }
    
    public string[] OmittedNs(string[] ns)
    {
        // Whoa, a programming interview whiteboard question in the wild!?
        // (Future employers please don't look.. it's almost midnight and I just want to generate some bindings before I got to sleep.. ;-;)
        return ns.Zip(NsPrefixToOmit)
            .Select(x => (x.First == x.Second) ? null : x.First)
            .Where(x => x != null)
            .Select(x => x!) // I wish NRTs worked better with Linq expressions like these...
            .Concat(ns.Length > NsPrefixToOmit.Count ? ns[NsPrefixToOmit.Count..] : Array.Empty<string>())
            .ToArray();
    }
    
    public string GetNamespacedName(string[] ns, string name)
    {
        string prefix = CUtil.GetNamespacePrefix(OmittedNs(ns));
        if (string.IsNullOrEmpty(prefix)) {
            return name;
        }
        return $"{prefix}_{name}";
    }
    
    public string GetNamespacedName(NamedDecl decl)
    {
        return GetNamespacedName(CUtil.GetNsFromCursor(decl), decl.Name);
    }
}