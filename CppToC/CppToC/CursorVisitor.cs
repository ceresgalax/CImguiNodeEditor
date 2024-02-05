using ClangSharp;
using ClangSharp.Interop;

namespace CppToC;

public static class CursorVisitor
{
    public static void Visit(Cursor cursor, Builder builder)
    {
        if (cursor is Decl decl) {
            DeclVisitor.Visit(decl, builder);
            return;
        }
        if (cursor is Stmt stmt) {
            StmtVisitor.Visit(stmt, builder);
            return;
        }
        
        switch (cursor.CursorKind) 
        {
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public static void VisitMany(IEnumerable<Cursor> cursors, Builder builder)
    {
        foreach (Cursor cursor in cursors) {
            Visit(cursor, builder);
        }
    }

    public static void VisitManyWithExceptions(IEnumerable<Cursor> cursors, IEnumerable<Cursor> exceptions, ICursorVisitor visitor)
    {
        foreach (Cursor cursor in cursors) {
            bool shouldSkip = false;
            foreach (Cursor exception in exceptions) {
                if (exception == cursor) {
                    shouldSkip = true;
                    break;
                }
            }

            if (shouldSkip) {
                continue;
            }

            visitor.Visit(cursor);
        }
    }
}