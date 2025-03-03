using ClangSharp;
using static ClangSharp.Interop.CX_DeclKind;

namespace CppToC;

public class ClangTypeUtil
{
    public static Decl? GetDeclaration(ClangSharp.Type type)
    {
        type = type.CanonicalType;
        
        if (type is TypedefType typedefType) {
            return typedefType.Decl;
        }
        
        if (type is TemplateSpecializationType tsType) {
            // This is correct, the C API's Type->Decl function also returns the template decl, not the template specialization decl.
            return tsType.TemplateName.AsTemplateDecl;
        }
        
        // This is as broad as we can go with the Clang C++ Api...
        TagDecl? tagDecl = type.AsTagDecl;
        if (tagDecl == null) {
            if (type.Handle.Declaration.DeclKind == CX_DeclKind_Invalid) {
                return null;
            }

            throw new NotImplementedException("Getting a type's declaration of this type is not supported yet.");
        }

        return tagDecl;
    }
}