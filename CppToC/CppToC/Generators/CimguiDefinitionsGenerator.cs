using System.Text.Json;
using System.Text.Json.Serialization;
using CppToC.Model;

namespace CppToC.Generators;

public class CimguiDefinition
{
    public class ArgType
    {
        [JsonPropertyName("name")] public string Name = "";
        [JsonPropertyName("type")] public string Type = "";
    }

    [JsonPropertyName("args")] public string Args = "";
    [JsonPropertyName("argsT")] public List<ArgType> ArgsT = new();
    [JsonPropertyName("argsoriginal")] public string ArgsOriginal = "";
    [JsonPropertyName("call_args")] public string CallArgs = "";
    [JsonPropertyName("cimguiname")] public string CimguiName = "";
    [JsonPropertyName("defaults")]  public Dictionary<string, object> Defaults = new();
    [JsonPropertyName("funcname")] public string FuncName = "";
    [JsonPropertyName("location")] public string Location = "";
    [JsonPropertyName("namespace")] public string Namespace = "";
    [JsonPropertyName("ov_cimguiname")] public string OvCimguiName = "";
    [JsonPropertyName("ret")] public string Ret = "";
    [JsonPropertyName("signature")] public string Signature = "";
    [JsonPropertyName("stname")] public string StName = "";
}

public static class CimguiDefinitionsGenerator
{

    public static void Generate(TextWriter writer, Builder builder)
    {
        Dictionary<string, CimguiDefinition> defs = new();

        foreach (FunctionData data in builder.Functions) {
            CimguiDefinition def = GetCimguiDefForFunction(data, selfOf: null);
            defs[def.OvCimguiName] = def;
        }

        foreach (RecordData record in builder.Records) {
            foreach (FunctionData method in record.Methods) {
                CimguiDefinition def = GetCimguiDefForFunction(method, selfOf: record);
                defs[def.OvCimguiName] = def;
            }
        }

        JsonSerializerOptions opts = new();
        opts.IncludeFields = true;
        opts.WriteIndented = true;
        writer.Write(JsonSerializer.Serialize(defs, opts));
    }

    public static CimguiDefinition GetCimguiDefForFunction(FunctionData data, RecordData? selfOf)
    {
        IEnumerable<string> parameters = CUtil.GetParameters(data);
        if (selfOf != null) {
            parameters = Enumerable.Repeat($"{CUtil.GetCType(selfOf.Type)}* __self", 1).Concat(parameters);
        }
        
        IEnumerable<string> cppArgs = data.Parameters
            .Select(p => $"{p.Type.ClangType.AsString} {p.Name}");

        IEnumerable<CimguiDefinition.ArgType> argsT = data.Parameters
            .Select(p => new CimguiDefinition.ArgType() { Name = p.Name, Type = CUtil.GetCType(p.Type.ClangType) });
        if (selfOf != null) {
            argsT = Enumerable.Repeat(new CimguiDefinition.ArgType
                    { Name = "__self", Type = $"{CUtil.GetCType(selfOf.Type)}*" }, 1)
                .Concat(argsT);
        }

        IEnumerable<string> sigParts = data.Parameters.Select(p => CUtil.GetCType(p.Type));
        if (selfOf != null) {
            sigParts = Enumerable.Repeat<string>($"{CUtil.GetCType(selfOf.Type)}*", 1).Concat(sigParts);
        }
        
        CimguiDefinition def = new() {
            Args = $"({string.Join(", ", parameters)})",
            ArgsT = argsT.ToList(),
            ArgsOriginal = $"({string.Join(",", cppArgs)})",
            CallArgs = $"({string.Join(",", data.Parameters.Select(p => p.Name))})",
            CimguiName = CUtil.GetCFunctionName(data, selfOf, withOverload: false),
            // TODO: Defaults
            FuncName = data.Name,
            Location = "", // TODO
            Namespace = string.Join("::", data.Namespace),
            OvCimguiName = CUtil.GetCFunctionName(data, selfOf),
            Ret = data.ReturnType == null ? "void" : CUtil.GetCType(data.ReturnType),
            Signature = $"({string.Join(",", sigParts)})",
            StName = "" // TODO
        };

        return def;
    }
    
}