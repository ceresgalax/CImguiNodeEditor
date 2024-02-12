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
    [JsonPropertyName("templated")] public bool Templated;
}

public static class CimguiDefinitionsGenerator
{

    public static void Generate(TextWriter writer, Builder builder)
    {
        Dictionary<string, List<CimguiDefinition>> defs = new();

        foreach (FunctionData data in builder.Functions) {
            CimguiDefinition def = GetCimguiDefForFunction(data, selfOf: null, isTemplated: false);

            if (!defs.TryGetValue(def.CimguiName, out List<CimguiDefinition>? defList)) {
                defList = new List<CimguiDefinition>();
                defs[def.CimguiName] = defList;
            }

            defList.Add(def);
        }

        RecordMethodOwner recordMethodOwner = new RecordMethodOwner(new RecordData());
        foreach (RecordData record in builder.Records) {
            recordMethodOwner.SetRecord(record);
            foreach (FunctionData method in record.Methods) {
                CimguiDefinition def = GetCimguiDefForFunction(method, recordMethodOwner, isTemplated: false);
                
                if (!defs.TryGetValue(def.CimguiName, out List<CimguiDefinition>? defList)) {
                    defList = new List<CimguiDefinition>();
                    defs[def.CimguiName] = defList;
                }

                defList.Add(def);
            }
        }
        
        foreach (TemplateRecordData templateRecord in builder.TemplateRecords.Values) {
            recordMethodOwner.SetRecord(templateRecord.Inner);
            foreach (FunctionData method in templateRecord.Inner.Methods) {
                CimguiDefinition def = GetCimguiDefForFunction(method, recordMethodOwner, isTemplated: true);
            
                if (!defs.TryGetValue(def.CimguiName, out List<CimguiDefinition>? defList)) {
                    defList = new List<CimguiDefinition>();
                    defs[def.CimguiName] = defList;
                }

                defList.Add(def);
            }
        }

        JsonSerializerOptions opts = new();
        opts.IncludeFields = true;
        opts.WriteIndented = true;
        writer.Write(JsonSerializer.Serialize(defs, opts));
    }

    public static CimguiDefinition GetCimguiDefForFunction(FunctionData data, RecordMethodOwner? selfOf, bool isTemplated)
    {
        IEnumerable<string> parameters = CUtil.GetParameters(data);
        string selfOfCName = "";
        if (selfOf != null) {
            selfOfCName = CUtil.GetNamespacedName(selfOf.Namespace, selfOf.Name);
            parameters = Enumerable.Repeat($"{selfOfCName}* __self", 1).Concat(parameters);
        }
        
        IEnumerable<string> cppArgs = data.Parameters
            .Select(p => $"{p.Type.ClangType.AsString} {p.Name}");

        IEnumerable<CimguiDefinition.ArgType> argsT = data.Parameters
            .Select(p => new CimguiDefinition.ArgType() { Name = p.Name, Type = CUtil.GetCType(p.Type.ClangType) });
        if (selfOf != null) {
            argsT = Enumerable.Repeat(new CimguiDefinition.ArgType
                    { Name = "__self", Type = $"{selfOfCName}*" }, 1)
                .Concat(argsT);
        }

        IEnumerable<string> sigParts = data.Parameters.Select(p => CUtil.GetCType(p.Type));
        if (selfOf != null) {
            sigParts = Enumerable.Repeat<string>($"{selfOfCName}*", 1).Concat(sigParts);
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
            StName = selfOf == null ? "" : CUtil.GetNamespacedName(selfOf),
            Templated = isTemplated
        };

        return def;
    }
    
}