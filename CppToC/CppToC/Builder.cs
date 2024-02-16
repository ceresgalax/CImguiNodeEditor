using ClangSharp;
using ClangSharp.Interop;
using CppToC.Model;

namespace CppToC;

public class Builder
{
    private HashSet<string> _normalizedSourcePaths = new();

    private readonly List<string> _namespaceStack = new();

    public readonly List<RecordData> Records = new();
    public readonly Dictionary<TemplateDecl, TemplateRecordData> TemplateRecords = new();
    public readonly List<EnumData> Enums = new();
    public readonly List<FunctionData> Functions = new();
    public readonly List<ForwardDeclaredRecordData> ForwardDeclaredRecords = new();

    public TemplateRecordData GetOrCreateTemplateRecordData(TemplateDecl templateDecl)
    {
        if (!TemplateRecords.TryGetValue(templateDecl, out TemplateRecordData? data)) {
            data = new TemplateRecordData();
            TemplateRecords[templateDecl] = data;
            data.Inner.Namespace = CUtil.GetNsFromCursor(templateDecl);
            data.Inner.Name = templateDecl.Name;
        }
        return data;
    }
    
    private static string NormalizePath(string? path)
    {
        if (string.IsNullOrEmpty(path)) {
            return "";
        }
        string fullPath = Path.GetFullPath(path).Replace('\\', '/');
        return fullPath;
    }
    
    public void SetSourcePaths(IEnumerable<string> paths)
    {
        _normalizedSourcePaths = paths.Select(NormalizePath).ToHashSet();
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