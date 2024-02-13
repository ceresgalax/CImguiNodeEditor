using ClangSharp;
using CppToC.Model;

namespace CppToC;

public class InstantiatedTemplateRecordMethodOwner : IMethodOwner
{
    private RecordData _inner;
    private TemplateArgumentSet _arguments;

    public InstantiatedTemplateRecordMethodOwner()
    {
        _inner = new RecordData();
        _arguments = new TemplateArgumentSet(Array.Empty<TemplateArgument>(), Array.Empty<TemplateArgumentSet>());
    }
    
    public InstantiatedTemplateRecordMethodOwner(RecordData inner, TemplateArgumentSet arguments)
    {
        _inner = inner;
        _arguments = arguments;
    }

    public void Set(RecordData inner, TemplateArgumentSet arguments)
    {
        _inner = inner;
        _arguments = arguments;
    }

    public string[] Namespace => _inner.Namespace;
    public string Name => _inner.Name;
    public IReadOnlyList<TemplateArgument> TemplateArguments => _arguments.Args;
}