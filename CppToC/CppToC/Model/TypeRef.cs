using ClangSharp.Interop;
using Type = ClangSharp.Type;

namespace CppToC.Model;

public class TypeRef
{
    public ClangSharp.Type ClangType;
    
    public TypeRef(ClangSharp.Type clangType)
    {
        ClangType = clangType;
    }
}