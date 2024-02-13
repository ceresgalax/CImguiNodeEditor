using ClangSharp;
using CppToC.Model;
using static ClangSharp.Interop.CXCursorKind;

namespace CppToC;

public class TemplateInstantiationVisitor : ICursorVisitor
{
    private readonly Builder _builder;
    private readonly List<TemplateArgumentSet> _currentTemplateArguments = new();
    
    public TemplateInstantiationVisitor(Builder builder)
    {
        _builder = builder;
    }

    public void Visit(Cursor cursor)
    {
        switch (cursor.CursorKind) {
            case CXCursor_FieldDecl:
                CheckType(((FieldDecl)cursor).Type);
                break;
            case CXCursor_CXXBaseSpecifier:
                CheckType(((CXXBaseSpecifier)cursor).Type);
                break;
            default:
                CursorVisitor.VisitManyWithExceptions(cursor.CursorChildren, Array.Empty<Cursor>(), this);
                break;
        }
    }
    
    private void CheckType(ClangSharp.Type type)
    {
        if (type is ElaboratedType elaboratedType) {
            type = elaboratedType.NamedType;
        }
        if (type is TemplateSpecializationType tsType) {
            ClassTemplateDecl decl = (ClassTemplateDecl) tsType.TemplateName.AsTemplateDecl;
            if (!_builder.IsDeclPartOfSourceFile(decl)) {
                return;
            }
            
            TemplateRecordData data = _builder.GetOrCreateTemplateRecordData(decl);

            TemplateArgumentSet newSet = new TemplateArgumentSet(tsType.Args, _currentTemplateArguments);

            if (data.Instantiations.Contains(newSet)) {
                return;
            }
            
            // Push to the stack.
            _currentTemplateArguments.Add(newSet);
            
            CursorVisitor.VisitManyWithExceptions(decl.CursorChildren, Array.Empty<Cursor>(), this);
            
            // Pop it off.
            _currentTemplateArguments.RemoveAt(_currentTemplateArguments.Count - 1);

            data.Instantiations.Add(newSet);
        }
    }
    
    
}