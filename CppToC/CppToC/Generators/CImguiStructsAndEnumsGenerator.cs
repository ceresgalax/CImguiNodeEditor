using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using CppToC.Model;

namespace CppToC.Generators;

public class CImguiStructsAndEnumsGenerator
{
    public class EnumEntry
    {
        [JsonPropertyName("calc_value")] public int CalcValue;
        [JsonPropertyName("name")] public string Name = "";
        [JsonPropertyName("value")] public string Value = "";
    }

    public class StructField
    {
        [JsonPropertyName("name")] public string Name = "";
        [JsonPropertyName("template_type")] public string TemplateType = "";
        [JsonPropertyName("type")] public string Type = "";
    }
    
    public class StructsAndEnums
    {
        [JsonPropertyName("enums")] public Dictionary<string, List<EnumEntry>> Enums = new();
        [JsonPropertyName("enumtypes")] public Dictionary<string, string> EnumTypes = new();
        [JsonPropertyName("locations")] public Dictionary<string, string> Locations = new();
        [JsonPropertyName("structs")] public Dictionary<string, List<StructField>> Structs = new();
        [JsonPropertyName("templated_structs")] public Dictionary<string, List<StructField>> TemplatedStructs = new();
        [JsonPropertyName("templates_done")] public Dictionary<string, Dictionary<string, bool>> TemplatesDone = new();
        [JsonPropertyName("typenames")] public Dictionary<string, string> Typenames = new();
    }
    
    public static void Generate(TextWriter writer, Builder builder)
    {
        StructsAndEnums outData = new();

        foreach (EnumData data in builder.Enums) {
            List<EnumEntry> entries = data.Constants
                .Select(c => new EnumEntry() {
                    Name = c.Name,
                    Value = c.Value.ToString(),
                    CalcValue = (int)c.Value
                })
                .ToList();

            string cName = CUtil.GetNamespacedName(data.Namespace, data.Name); 
            
            outData.Enums[cName] = entries;
            outData.EnumTypes[cName] = "int"; // TODO
            outData.Locations[cName] = ""; // TODO
        }

        foreach (RecordData record in builder.Records) {
            List<StructField> fields = record.Fields
                .Select(f => new StructField {
                    Name = f.Name,
                    Type = CUtil.GetCType(f.Type)
                })
                .ToList();

            string cName = CUtil.GetNamespacedName(record.Namespace, record.Name, record.TemplateArgs);
            outData.Structs[cName] = fields;
            outData.Locations[cName] = ""; // TODO
        }

        JsonSerializerOptions opts = new();
        opts.IncludeFields = true;
        opts.WriteIndented = true;
        writer.Write(JsonSerializer.Serialize(outData, opts));
    }
}