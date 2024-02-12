using ClangSharp;

namespace CppToC;

public interface IMethodOwner
{
    public string[] Namespace { get; }
    public string Name { get; }
    public IReadOnlyList<TemplateArgument> TemplateArguments { get; }
}