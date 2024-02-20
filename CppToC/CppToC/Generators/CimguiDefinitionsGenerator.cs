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

    public static void Generate(TextWriter writer, Builder builder, string[] omittedNsPrefix)
    {
        Dictionary<string, List<CimguiDefinition>> defs = new();

        CTypeTranslator translator = new ();
        translator.NsPrefixToOmit = omittedNsPrefix;

        void AddDef(FunctionData funcData, IMethodOwner? methodOwner)
        {
            if (funcData.IsOperator) {
                // TODO: Omitting operators should be an option, and omitting should be off by default.
                // Some generators may handle operators just fine.
                Console.WriteLine($"{nameof(CimguiDefinitionsGenerator)}: Omitting operator {funcData.Name}, as binding generators typically don't know how to wrap such methods.");
                return;
            }
            
            CimguiDefinition def = GetCimguiDefForFunction(translator, funcData, methodOwner, isTemplated: false);
            
            string name = translator.GetCFunctionName(funcData, methodOwner);
            if (!defs.TryGetValue(name, out List<CimguiDefinition>? defList)) {
                defList = new List<CimguiDefinition>();
                defs[name] = defList;
            }

            defList.Add(def);
        }
        
        foreach (FunctionData data in builder.Functions) {
            AddDef(data, methodOwner: null);
        }

        RecordMethodOwner recordMethodOwner = new RecordMethodOwner(new RecordData());
        foreach (RecordData record in builder.Records) {
            recordMethodOwner.SetRecord(record);
            foreach (FunctionData method in record.Methods) {
                AddDef(method, recordMethodOwner);
            }
        }
        
        InstantiatedTemplateRecordMethodOwner itrMethodOwner = new();
        foreach (TemplateRecordData templateRecord in builder.TemplateRecords.Values) {
            foreach (TemplateArgumentSet set in templateRecord.Instantiations) {
                itrMethodOwner.Set(templateRecord.Inner, set);
                translator.PushTemplateArgumentSet(set);

                foreach (FunctionData method in templateRecord.Inner.Methods) {
                    AddDef(method, itrMethodOwner);
                }

                translator.PopTemplateArgumentSet();
            }
        }

        JsonSerializerOptions opts = new();
        opts.IncludeFields = true;
        opts.WriteIndented = true;
        writer.Write(JsonSerializer.Serialize(defs, opts));
    }
    
    public static CimguiDefinition GetCimguiDefForFunction(CTypeTranslator translator, FunctionData data, IMethodOwner? selfOf, bool isTemplated)
    {
        bool shouldPassReturNValueAsOutParameter = false;
        string outParameterType = "";
        
        if (data.ReturnType != null) {
            shouldPassReturNValueAsOutParameter = translator.ShouldPassReturnValueAsOutParameter(data.ReturnType.ClangType);
            outParameterType = translator.GetCType(data.ReturnType);
        }
        
        IEnumerable<string> parameters = translator.GetParameters(data);
        string selfOfCName = "";
        if (selfOf != null) {
            selfOfCName = translator.GetNamespacedName(selfOf.Namespace, selfOf.Name, selfOf.TemplateArguments);
            parameters = Enumerable.Repeat($"{selfOfCName}* __self", 1).Concat(parameters);
        }
        if (shouldPassReturNValueAsOutParameter) {
            parameters = parameters.Concat(Enumerable.Repeat($"{outParameterType}* pOut", 1));
        }
        
        IEnumerable<string> cppArgs = data.Parameters
            .Select(p => $"{p.Type.ClangType.AsString} {p.Name}");

        IEnumerable<CimguiDefinition.ArgType> argsT = data.Parameters
            .Select(p => new CimguiDefinition.ArgType { Name = p.Name, Type = translator.GetCType(p.Type.ClangType) });
        if (selfOf != null) {
            argsT = Enumerable.Repeat(new CimguiDefinition.ArgType { Name = "__self", Type = $"{selfOfCName}*" }, 1)
                .Concat(argsT);
        }
        if (shouldPassReturNValueAsOutParameter) {
            argsT = argsT.Concat(Enumerable.Repeat(new CimguiDefinition.ArgType {
                Name = "pOut",
                Type = $"{outParameterType}*"
            }, 1));
        }

        IEnumerable<string> sigParts = data.Parameters.Select(p => translator.GetCType(p.Type));
        
        if (selfOf != null) {
            sigParts = Enumerable.Repeat<string>($"{selfOfCName}*", 1).Concat(sigParts);
        }
        if (shouldPassReturNValueAsOutParameter) {
            sigParts = sigParts.Concat(Enumerable.Repeat<string>($"{outParameterType}* pOut", 1));    
        }
        

        CTypeTranslator cimguiTranslator = new(translator);
        cimguiTranslator.NsPrefixToOmit = Array.Empty<string>();
        
        CimguiDefinition def = new() {
            Args = $"({string.Join(", ", parameters)})",
            ArgsT = argsT.ToList(),
            ArgsOriginal = $"({string.Join(",", cppArgs)})",
            CallArgs = $"({string.Join(",", data.Parameters.Select(p => p.Name))})",
            CimguiName = cimguiTranslator.GetCFunctionName(data, selfOf, withOverload: false),
            // TODO: Defaults
            FuncName = data.Name,
            Location = "", // TODO
            Namespace = string.Join("::", data.Namespace),
            OvCimguiName = cimguiTranslator.GetCFunctionName(data, selfOf),
            Ret = data.ReturnType == null || shouldPassReturNValueAsOutParameter ? "void" : translator.GetCType(data.ReturnType),
            Signature = $"({string.Join(",", sigParts)})",
            StName = selfOf == null ? "" : translator.GetNamespacedName(selfOf.Namespace, selfOf.Name, selfOf.TemplateArguments),
            Templated = isTemplated
        };

        return def;
    }
    
}