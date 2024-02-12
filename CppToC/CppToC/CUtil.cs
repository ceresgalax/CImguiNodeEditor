using System.ComponentModel;
using System.Text;
using ClangSharp;
using ClangSharp.Interop;
using CppToC.Model;
using static ClangSharp.Interop.CX_TypeClass;
using static ClangSharp.Interop.CXTypeKind;

namespace CppToC;

public class CUtil
{
    // TODO: Refactor to be non-global once this is proven to work.
    public static readonly List<TemplateArgumentSet> TemplateArgumentStack = new();
    
    public static string GetNamespacePrefix(string[] ns)
    {
        return string.Join("", ns);
    }

    public static string GetNamespacedName(string[] ns, string name)
    {
        string prefix = GetNamespacePrefix(ns);
        if (string.IsNullOrEmpty(prefix)) {
            return name;
        }
        return $"{prefix}_{name}";
    }

    public static string GetNamespacedName(string[] ns, string name, IReadOnlyList<TemplateArgument> templateArgs)
    {
        string prefix = GetNamespacedName(ns, name);
        string postfix = GetTemplateParamCtypePostfix(templateArgs);
        if (string.IsNullOrEmpty(postfix)) {
            return prefix;
        }
        return $"{prefix}_{postfix}";
    }

    public static string GetNamespacedName(IMethodOwner methodOwner)
    {
        return GetNamespacedName(methodOwner.Namespace, methodOwner.Name, methodOwner.TemplateArguments);
    }

    public static string GetNamespacedName(NamedDecl decl)
    {
        return GetNamespacedName(GetNsFromCursor(decl), decl.Name);
    }

    public static string GetCType(TypeRef type)
    {
        return GetCType(type.ClangType);
    }

    public static IEnumerable<string> GetParameters(FunctionData data)
    {
        return data.Parameters.Select(p => $"{GetCType(p.Type.ClangType)} {p.Name}");
    }
    
    public static string GetCType(ClangSharp.Type type)
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
                string prefix = "";
                if (type.IsLocalConstQualified) {
                    prefix = "const ";
                }
                return prefix + GetNamespacedName(GetNsFromCursor(tagType.Decl), tagType.Decl.Name);
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
                return $"{GetCType(arrayType.ElementType)}[{arrayType.Size}]";
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
                return GetCType(et.NamedType);
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

    public static string GetTypedefCType(TypedefType type)
    {
        if (type.AsString == "uintptr_t" || type.AsString == "size_t") {
            return "void*";
        }
        return GetCType(type.Desugar);
    }

    public static string GetUnexposedCType(ClangSharp.Type type)
    {
        switch (type.TypeClass) {
            case CX_TypeClass_SubstTemplateTypeParm: {
                SubstTemplateTypeParmType substType = (SubstTemplateTypeParmType)type;
                return GetCType(substType.ReplacementType);
            }
            case CX_TypeClass_TemplateSpecialization: {
                TemplateSpecializationType specializationType = (TemplateSpecializationType)type;
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
            default:
                throw new NotImplementedException();
        }
    }

    public static string GetTemplateSpecializationCType(NamedDecl templateDecl, IReadOnlyList<TemplateArgument> args)
    {
        return $"{GetNamespacedName(templateDecl)}_{GetTemplateParamCtypePostfix(args)}";
    }

    public static string GetTemplateParamCtypePostfix(IReadOnlyList<TemplateArgument> args)
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
    
    public static string GetTemplateSpecializationCType(ClassTemplateSpecializationDecl decl)
    {
        return GetTemplateSpecializationCType(decl, decl.TemplateArgs);
    }

    public static string[] GetNsFromCursor(Cursor cursor)
    {
        Stack<string> parts = new();
        Cursor? lexicalParent = cursor.LexicalParentCursor;
        while (lexicalParent != null) {
            if (lexicalParent.CursorKind == CXCursorKind.CXCursor_TranslationUnit) {
                break;
            }
            parts.Push(lexicalParent.Spelling);
            lexicalParent = lexicalParent.LexicalParentCursor;
        }
        return parts.ToArray();
    }

    public static string GetOperatorOverloadName(CX_OverloadedOperatorKind kind)
    {
        return Enum.GetName(kind)?.Replace("CX_OO_", "").ToLowerInvariant() ?? throw new InvalidEnumArgumentException(nameof(kind), (int)kind, typeof(CX_OverloadedOperatorKind));
    }

    public static string GetRecordCppSpelling(RecordData record)
    {
        return GetRecordCppSpelling(record.Namespace, record.Name, Array.Empty<TemplateArgument>());
    }
    
    public static string GetRecordCppSpelling(string[] ns, string name, IReadOnlyList<TemplateArgument> templateArgs)
    {
        StringBuilder builder = new();
        
        if (ns.Length > 0) {
            foreach (string part in ns) {
                builder.Append(part);
                builder.Append("::");
            }
        }

        builder.Append(name);
        
        if (templateArgs.Count > 0) {
            builder.Append('<');

            for (int i = 0, ilen = templateArgs.Count; i < ilen; ++i) {
                TemplateArgument arg = templateArgs[i];
                builder.Append(arg.AsType.AsString);
                if (i + 1 < ilen) {
                    builder.Append(", ");
                }
            }
            builder.Append('>');
        }

        return builder.ToString();
    }

    public static string GetCppFunctionSpelling(FunctionData data)
    {
        StringBuilder builder = new();
        builder.Append(GetNamespacedCppName(data.Namespace, data.Name));
        
        if (data.TemplateArgs.Length > 0) {
            builder.Append('<');

            for (int i = 0, ilen = data.TemplateArgs.Length; i < ilen; ++i) {
                TemplateArgument arg = data.TemplateArgs[i];
                builder.Append(arg.AsType.AsString);
                if (i + 1 < ilen) {
                    builder.Append(", ");
                }
            }
            builder.Append('>');
        }

        return builder.ToString();
    }
    
    public static string GetCFunctionName(FunctionData function, IMethodOwner? selfOf, bool withOverload = true)
    {
        string name;
        if (function.IsOperator) {
            name = "operator_" + GetOperatorOverloadName(function.OverloadedOperator);
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

    public static string GetCFunctionLine(FunctionData data, IMethodOwner? selfOf)
    {
        string returnType = "void";
        if (data.ReturnType != null) {
            returnType = GetCType(data.ReturnType);
        }

        string name = GetCFunctionName(data, selfOf);

        IEnumerable<string> selfPrefix = Array.Empty<string>();
        if (selfOf != null) {
            // TODO: If this is a const function, make the self pointer const.
            selfPrefix = Enumerable.Repeat($"{GetNamespacedName(selfOf)}* __self", 1);
        }
        
        string parameters = string.Join(", ", selfPrefix.Concat(GetParameters(data)));
        return $"{returnType} {name}({parameters})";
    }

    public static string GetNamespacedCppName(string[] ns, string name)
    {
        StringBuilder builder = new();
        
        if (ns.Length > 0) {
            foreach (string part in ns) {
                builder.Append(part);
                builder.Append("::");
            }
        }
        
        builder.Append(name);
        return builder.ToString();
    }
    
}