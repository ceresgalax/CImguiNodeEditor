using ClangSharp;
using CppToC.Model;

namespace CppToC;

public class RecordMethodOwner : IMethodOwner
{
    private RecordData _record;
    
    public RecordMethodOwner(RecordData record)
    {
        _record = record;
    }

    public void SetRecord(RecordData record)
    {
        _record = record;
    }

    public string[] Namespace => _record.Namespace;
    public string Name => _record.Name;
    public IReadOnlyList<TemplateArgument> TemplateArguments => Array.Empty<TemplateArgument>();
}