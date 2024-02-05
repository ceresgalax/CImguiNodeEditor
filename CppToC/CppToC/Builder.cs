using ClangSharp;
using ClangSharp.Interop;
using CppToC.Model;

namespace CppToC;

public class Builder
{
    private readonly HashSet<string> _normalizedSourcePaths = new();

    private readonly List<string> _namespaceStack = new();

    public readonly List<RecordData> Records = new();
    public readonly List<EnumData> Enums = new();
    public readonly List<FunctionData> Functions = new();
    public readonly List<ForwardDeclaredRecordData> ForwardDeclaredRecords = new();

    private static string NormalizePath(string? path)
    {
        if (string.IsNullOrEmpty(path)) {
            return "";
        }
        string fullPath = Path.GetFullPath(path).Replace('\\', '/');
        return fullPath;
    }
    
    public void AddSourcePath(string path)
    {
        _normalizedSourcePaths.Add(NormalizePath(path));
    }

    public void PushNamespace(string name)
    {
        _namespaceStack.Add(name);
    }

    public void PopNamespace()
    {
        _namespaceStack.RemoveAt(_namespaceStack.Count - 1);
    }

    public string[] GetCurrentNamespace()
    {
        return _namespaceStack.ToArray();
    }

    
    public bool IsDeclPartOfSourceFile(Decl decl)
    {
        decl.Location.GetFileLocation(out CXFile file, out uint _, out uint _, out uint _);
        string path = NormalizePath(file.Name.ToString());
        return _normalizedSourcePaths.Contains(path);
    }
    
    
    
}