using ClangSharp;
using ClangSharp.Interop;
using CppToC.Model;
using static ClangSharp.Interop.CX_DeclKind;
using static ClangSharp.Interop.CXCursorKind;

namespace CppToC;

public class RecordVisitor : ICursorVisitor
{
    public RecordData Data = new();
    private readonly Builder _builder;
    
    public RecordVisitor(Builder builder)
    {
        _builder = builder;
    }

    public void Visit(Cursor cursor)
    {
        if (cursor is Decl decl) {
            switch (decl.Kind) {
                case CX_DeclKind_ClassTemplateSpecialization:
                    VisitClassTemplateSpecialization((ClassTemplateSpecializationDecl)cursor);
                    break;
                case CX_DeclKind_CXXRecord:
                    VisitRecord((CXXRecordDecl)decl);
                    break;
                case CX_DeclKind_FunctionTemplate:
                    VisitFunctionTemplate((FunctionTemplateDecl)cursor);
                    break;    
                case CX_DeclKind_CXXMethod:
                    VisitMethod((CXXMethodDecl)cursor);
                    break;
                case CX_DeclKind_CXXConversion:
                    VisitConversion((CXXConversionDecl)cursor);
                    break;
                case CX_DeclKind_Field:
                    VisitField((FieldDecl)cursor);
                    break;
                case CX_DeclKind_AccessSpec:
                    // No-op for now.
                    break;
                case CX_DeclKind_Var:
                    // Static fields in the record. 
                    // No-op for now.
                    break;
                case CX_DeclKind_UnresolvedUsingValue:
                case CX_DeclKind_TypeAlias:
                    // We don't care about these. No-op for now.
                    break;
                case CX_DeclKind_Using:
                    VisitUsingDecl((UsingDecl)decl);
                    break;
                case CX_DeclKind_CXXConstructor:
                    VisitConstructor((CXXConstructorDecl)cursor);
                    break;
                case CX_DeclKind_CXXDestructor:
                    VisitDestructor((CXXDestructorDecl)cursor);
                    break;
                case CX_DeclKind_ConstructorUsingShadow:
                    VisitConstructorUsingShadow((ConstructorUsingShadowDecl)cursor);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return;
        }
        
        switch (cursor.CursorKind) {
            case CXCursor_CXXBaseSpecifier:
                VisitBaseSpecifier((CXXBaseSpecifier)cursor);
                break;
            case CXCursor_CXXFinalAttr:
                // We don't keep track of this.
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void VisitClassTemplateSpecialization(ClassTemplateSpecializationDecl decl)
    {
        Data.TemplateArgs = decl.TemplateArgs.ToArray();
        
        // Oddly, these cursors do not have children. Maybe that is a ClangSharp bug?
        // Anyways, use the properties instead to gather the members of the class template specialization.
        CursorVisitor.VisitManyWithExceptions(decl.Decls, Array.Empty<Cursor>(), this);
        CursorVisitor.VisitManyWithExceptions(decl.Bases, Array.Empty<Cursor>(), this);
        
        //VisitRecord(decl);
    }

    public void VisitRecord(CXXRecordDecl decl)
    {
        if (!string.IsNullOrEmpty(Data.Name)) {
            throw new InvalidOperationException("Visiting a record twice!?");
        }
        Data.Type = new TypeRef(decl.TypeForDecl);
        Data.Name = decl.Name;
        Data.Namespace = _builder.GetCurrentNamespace();
        
        CursorVisitor.VisitManyWithExceptions(decl.CursorChildren, Array.Empty<Cursor>(), this);
    }
    
    public void VisitConstructor(CXXConstructorDecl decl)
    {
        FunctionData data = new();
        ParseParameters(data, decl.Parameters);
        Data.Constructors.Add(data);
    }

    public void VisitDestructor(CXXDestructorDecl decl)
    {
        FunctionData data = new();
        ParseParameters(data, decl.Parameters);
        Data.Destructors.Add(data);
    }

    public void VisitConstructorUsingShadow(ConstructorUsingShadowDecl decl)
    {
        // Ignore for now. Uninherited constructor.
    }

    public void VisitFunctionTemplate(FunctionTemplateDecl decl)
    {
        // if (decl.IsThisDeclarationADefinition && decl.Body == null) {
        //     // This must be explicitley deleted. (Not sure how default is defined?)
        //     return;
        // }

        foreach (FunctionDecl function in decl.Specializations) {
            throw new NotImplementedException();
        }
        
        // FunctionTemplateData data = new();
        // DeclVisitor.ParseFunctionTemplate(data, decl);
        // Data.TemplateMethods.Add(data);
    }

    public void VisitMethod(CXXMethodDecl decl)
    {
        FunctionData data = new();
        data.Name = decl.Name;
        data.ReturnType = new TypeRef(decl.ReturnType);
        ParseParameters(data, decl.Parameters);

        if (decl.IsOverloadedOperator) {
            data.IsOperator = true;
            data.OverloadedOperator = decl.OverloadedOperator;
        }
        
        Data.Methods.Add(data);
    }

    public static void ParseParameters(FunctionData data, IEnumerable<ParmVarDecl> parameters)
    {
        int index = 0;
        foreach (ParmVarDecl param in parameters) {
            data.Parameters.Add(new ParameterData {
                Name = string.IsNullOrEmpty(param.Name) ? $"__{index}" : param.Name,
                Type = new TypeRef(param.Type)
            });

            ++index;
        }
    }

    public void VisitConversion(CXXConversionDecl decl)
    {
        FunctionData data = new();
        ParseParameters(data, decl.Parameters);
        Data.Conversions.Add(data);
    }

    public void VisitField(FieldDecl decl)
    {
        FieldData data = new(decl.Name, new TypeRef(decl.Type));
        Data.Fields.Add(data);
    }

    public void VisitBaseSpecifier(CXXBaseSpecifier baseSpecifier)
    {
        Data.BaseTypes.Add(new TypeRef(baseSpecifier.Type));
    }

    public void VisitUsingDecl(UsingDecl decl)
    {
        foreach (Cursor cursor in decl.CursorChildren) {
            if (cursor is not OverloadedDeclRef declRef) {
                continue;
            }

            foreach (Decl overloadedDecl in declRef.OverloadedDecls) {
                Visit(overloadedDecl);
            }

            break;
        }
    }

}