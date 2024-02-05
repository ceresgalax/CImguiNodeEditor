using ClangSharp;
using ClangSharp.Interop;

namespace CppToC.Model;

public class FunctionData
{
    public string Name = "";
    public string[] Namespace = Array.Empty<string>();
    public List<ParameterData> Parameters = new();
    public TypeRef? ReturnType;
    public bool IsOperator;
    public CX_OverloadedOperatorKind OverloadedOperator;
    
    /// <summary>
    /// If this function was instantiated by a template, these are the template arguments that were used to instantiate it!
    /// </summary>
    public TemplateArgument[] TemplateArgs = Array.Empty<TemplateArgument>();

    public int OverloadIndex;
}