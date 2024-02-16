using ClangSharp;
using ClangSharp.Interop;
using CppToC.Model;
using static ClangSharp.Interop.CX_DeclKind;

namespace CppToC;

public static class DeclVisitor
{
    public static void Visit(Decl decl, Builder builder)
    {
        if (decl.Kind != CX_DeclKind_TranslationUnit) {
            if (!builder.IsDeclPartOfSourceFile(decl)) {
                return;
            }    
        }
        
        switch (decl.Kind) 
        {
            case CX_DeclKind_TranslationUnit:
                VisitTranslationUnit((TranslationUnitDecl)decl, builder);
                break;
            case CX_DeclKind_Namespace:
                VisitNamespace((NamespaceDecl)decl, builder);
                break;
            case CX_DeclKind_CXXRecord:
                VisitCXXRecord((CXXRecordDecl)decl, builder);
                break;
            case CX_DeclKind_Enum:
                VisitEnum((EnumDecl)decl, builder);
                break;
            case CX_DeclKind_Function:
                VisitFunction((FunctionDecl)decl, builder);
                break;
            case CX_DeclKind_TypeAlias:
                VisitTypeAlias((TypeAliasDecl) decl, builder);
                break;
            case CX_DeclKind_ClassTemplate:
                VisitClassTemplate((ClassTemplateDecl)decl, builder);
                break;
            case CX_DeclKind_Var:
                // No-op for now.
                break;
            case CX_DeclKind_FunctionTemplate:
                
                break;
            case CX_DeclKind_Using:
                // No-op.
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public static void VisitTranslationUnit(TranslationUnitDecl decl, Builder builder)
    {
        CursorVisitor.VisitMany(decl.CursorChildren, builder);
    }

    public static void VisitNamespace(NamespaceDecl decl, Builder builder)
    {
        if (decl.IsAnonymousNamespace) {
            // TODO: Log
            return;
        } else {
            builder.PushNamespace(decl.Name);    
        }
        
        CursorVisitor.VisitMany(decl.CursorChildren, builder);

        if (!decl.IsAnonymousNamespace) {
            builder.PopNamespace();
        }
    }

    public static void VisitCXXRecord(CXXRecordDecl decl, Builder builder)
    {
        if (!decl.IsCompleteDefinition) {
            if (!decl.HasDefinition) {
                builder.ForwardDeclaredRecords.Add(new ForwardDeclaredRecordData() {
                    Namespace = builder.GetCurrentNamespace(),
                    Name = decl.Name
                });
                return;
            }
            
            // Just a forward declaration that we'll see the definition for later. (or may have already seen!)
            return;
        }

        
        RecordVisitor visitor = new(builder);
        visitor.Visit(decl);
        builder.Records.Add(visitor.Data);
        
        // Check if this record needs us to instantiate any template records.
        TemplateInstantiationVisitor templateInstantiationVisitor = new(builder);
        templateInstantiationVisitor.Visit(decl);
        CursorVisitor.VisitManyWithExceptions(decl.Bases, Array.Empty<Cursor>(), templateInstantiationVisitor);
    }

    public static void VisitEnum(EnumDecl decl, Builder builder)
    {
        EnumData enumData = new() {
            Name = decl.Name,
            Namespace = builder.GetCurrentNamespace()
        };

        foreach (Decl childDecl in decl.Decls) {
            EnumConstantDecl constDecl = (EnumConstantDecl)childDecl;

            EnumConstant constData = new() {
                Name = constDecl.Name,
                Value = constDecl.InitVal
            };
            
            enumData.Constants.Add(constData);
        }
        
        builder.Enums.Add(enumData);
    }

    public static void VisitFunction(FunctionDecl decl, Builder builder)
    {
        builder.Functions.Add(ParseFunction(decl, builder.GetCurrentNamespace()));
    }

    public static FunctionData ParseFunction(FunctionDecl decl, string[] ns)
    {
        FunctionData data = new();
        data.Name = decl.Name;
        data.Namespace = ns;
        data.ReturnType = new TypeRef(decl.ReturnType);
        RecordVisitor.ParseParameters(data, decl.Parameters);
        if (decl.IsOverloadedOperator) {
            data.IsOperator = true;
            data.OverloadedOperator = decl.OverloadedOperator;
        }
        return data;
    }

    public static void VisitFunctionTemplate(FunctionTemplateDecl decl, Builder builder)
    {
        foreach (FunctionDecl function in decl.Specializations) {
            throw new NotImplementedException();
        }
        // FunctionTemplateData data = new();
        // ParseFunctionTemplate(data, decl);
        // builder.FunctionTemplates.Add(data);
    }

    public static void VisitTypeAlias(TypeAliasDecl decl, Builder builder)
    {
        // Not sure how to best handle these.
        //throw new NotImplementedException();
    }

    public static void VisitClassTemplate(ClassTemplateDecl decl, Builder builder)
    {
        RecordVisitor recordVisitor = new(builder);
        CursorVisitor.VisitManyWithExceptions(decl.CursorChildren, decl.TemplateParameters, recordVisitor);
        RecordData data = recordVisitor.Data;
        TemplateRecordData templateRecordData = builder.GetOrCreateTemplateRecordData(decl);
        templateRecordData.Parameters = decl.TemplateParameters;
        templateRecordData.Inner = data;
        data.Namespace = builder.GetCurrentNamespace();
        data.Name = decl.Name;
    }

    public static void VisitVarDecl(VarDecl decl, Builder builder)
    {
        if (decl.IsStaticDataMember) {
            // Log?
            return;
        }
        
        // TODO
        throw new NotImplementedException();
    }

}