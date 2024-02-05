using ClangSharp;

namespace CppToC;

public interface ICursorVisitor
{
    void Visit(Cursor cursor);
}