using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using ClangSharp;
using ClangSharp.Interop;
using CppToC.Model;
using static ClangSharp.Interop.CXTemplateArgumentKind;

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
        
        [JsonPropertyName("type")] public string Type = "";
        
        /// <summary>
        /// If type describes a template, this is the type name of the template parameter(s).
        /// (Note that Type will still contain the full template name (e.g: ImVector_char for ImVector&lt;char&gt;. )
        /// </summary>
        [JsonPropertyName("template_type")] public string TemplateType = "";
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
            string cName = CUtil.GetNamespacedName(record.Namespace, record.Name, Array.Empty<TemplateArgument>());
            outData.Structs[cName] = GetStructFields(record);
            outData.Locations[cName] = ""; // TODO
        }

        foreach (ForwardDeclaredRecordData record in builder.ForwardDeclaredRecords) {
            string cName = CUtil.GetNamespacedName(record.Namespace, record.Name);
            outData.Structs[cName] = new List<StructField>();
            outData.Locations[cName] = ""; // TODO
        }

        foreach (TemplateRecordData templateRecordData in builder.TemplateRecords.Values) {
            string name = CUtil.GetNamespacedName(templateRecordData.Inner.Namespace, templateRecordData.Inner.Name);
            outData.TemplatedStructs[name] = GetStructFields(templateRecordData.Inner);
            outData.Locations[name] = ""; // TODO

            string GetNTTPDString(NonTypeTemplateParmDecl decl)
            {
                string s = $"{decl.Type.AsString} {decl.Name}";
                if (decl.HasDefaultArgument) {
                    s += $" = {decl.DefaultArgument.Spelling}";
                }
                return s;
            }
            
            string GetParameterString(NamedDecl decl)
            {
                return decl switch {
                    TemplateTypeParmDecl typeParm => typeParm.Name,
                    NonTypeTemplateParmDecl nonTypeParm => GetNTTPDString(nonTypeParm),
                    _ => throw new NotImplementedException()
                }; 
            }
            
            outData.Typenames[name] = string.Join(", ", templateRecordData.Parameters.Select(GetParameterString));

            Dictionary<string, bool> done = new();
            outData.TemplatesDone[name] = done;
            
            foreach (TemplateArgumentSet set in templateRecordData.Instantiations) {
                CUtil.TemplateArgumentStack.Add(set);
                string argsString = string.Join(", ", set.Args.Select(GetArgString));
                done[argsString] = true;
                CUtil.TemplateArgumentStack.RemoveAt(CUtil.TemplateArgumentStack.Count - 1);
            }
        }

        JsonSerializerOptions opts = new();
        opts.IncludeFields = true;
        opts.WriteIndented = true;
        writer.Write(JsonSerializer.Serialize(outData, opts));
    }
    
    private static string GetArgString(TemplateArgument arg)
    {
        return arg.Kind switch {
            CXTemplateArgumentKind_Type => CUtil.GetCType(arg.AsType),
            CXTemplateArgumentKind_Integral => arg.AsIntegral.ToString(),
            _ => throw new NotImplementedException()
        };
    }

    private static List<StructField> GetStructFields(RecordData record)
    {
        string GetTemplateType(FieldData field)
        {
            // if (field.Type.ClangType is TemplateSpecializationType tst) {
            //     return string.Join(", ", tst.Args
            //         .Select(GetArgString)
            //     );
            // }
            return "";
        }
        
        return record.Fields
            .Select(f => new StructField {
                Name = f.Name,
                Type = CUtil.GetCType(f.Type),
                TemplateType = GetTemplateType(f)
            })
            .ToList();
    }
}