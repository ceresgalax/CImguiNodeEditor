using ClangSharp;

namespace CppToC.Model;

public class RecordData
{
    public TypeRef Type;
    public string Name = "";
    public string[] Namespace = Array.Empty<string>();
    public List<FieldData> Fields = new();

    public List<FunctionData> Constructors = new();
    public List<FunctionData> Destructors = new();
    public List<FunctionData> Methods = new();
    
    /// <summary>
    /// Things like user-defined explicit or implicit conversion
    /// e.g: `explicit operator T() const { return ... }`
    /// </summary>
    public List<FunctionData> Conversions = new();

    public List<TypeRef> BaseTypes = new();

    /// <summary>
    /// If this record was instantiated by a template, these are the template arguments that were used to instantiate it!
    /// </summary>
    public TemplateArgument[] TemplateArgs = Array.Empty<TemplateArgument>();
}