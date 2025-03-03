using System.Text.Json;
using System.Text.Json.Serialization;
using ClangSharp;
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

        [JsonPropertyName("size")] public int Size;
        
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
    
    public static void Generate(TextWriter writer, Builder builder, string[] nsPrefixToOmit)
    {
        CTypeTranslator cTypeTranslator = new();
        cTypeTranslator.NsPrefixToOmit = nsPrefixToOmit;
        
        StructsAndEnums outData = new();

        foreach (EnumData data in builder.Enums) {
            List<EnumEntry> entries = data.Constants
                .Select(c => new EnumEntry() {
                    Name = c.Name,
                    Value = c.Value.ToString(),
                    CalcValue = (int)c.Value
                })
                .ToList();

            string cName = cTypeTranslator.GetNamespacedName(data.Namespace, data.Name); 
            
            outData.Enums[cName] = entries;
            outData.EnumTypes[cName] = "int"; // TODO
            outData.Locations[cName] = ""; // TODO
        }

        foreach (RecordData record in builder.Records) {
            string cName = cTypeTranslator.GetNamespacedName(record.Namespace, record.Name, Array.Empty<TemplateArgument>());
            outData.Structs[cName] = GetStructFields(record, cTypeTranslator, builder);
            outData.Locations[cName] = ""; // TODO
        }

        foreach (ForwardDeclaredRecordData record in builder.ForwardDeclaredRecords) {
            string cName = cTypeTranslator.GetNamespacedName(record.Namespace, record.Name);
            outData.Structs[cName] = new List<StructField>();
            outData.Locations[cName] = ""; // TODO
        }
        
        //
        // CImgui doesn't handle template classes very well.
        // More specifically, the Imgui.NET generator doesn't know how to create template classes, all templated classes
        // in ImGui.NET are special case. (And translate the ImGui containers to the .NET System equivalents)
        // This is understandable, as the cimgui json "schema" doesn't define mappings from template parameters to 
        // instantiated c class names.
        //

        foreach (TemplateRecordData templateRecordData in builder.TemplateRecords.Values) {
            foreach (TemplateArgumentSet set in templateRecordData.Instantiations) {
                cTypeTranslator.PushTemplateArgumentSet(set);
                
                string cName = cTypeTranslator.GetNamespacedName(templateRecordData.Inner.Namespace, templateRecordData.Inner.Name, set.Args);

                outData.Structs[cName] = GetStructFields(templateRecordData.Inner, cTypeTranslator, builder);
                outData.Locations[cName] = ""; // TODO
                
                cTypeTranslator.PopTemplateArgumentSet();
            }
        }

        JsonSerializerOptions opts = new();
        opts.IncludeFields = true;
        opts.WriteIndented = true;
        writer.Write(JsonSerializer.Serialize(outData, opts));
    }
    
    private static string GetArgString(TemplateArgument arg, CTypeTranslator translator)
    {
        return arg.Kind switch {
            CXTemplateArgumentKind_Type => translator.GetCType(arg.AsType),
            CXTemplateArgumentKind_Integral => arg.AsIntegral.ToString(),
            _ => throw new NotImplementedException()
        };
    }

    private static List<StructField> GetStructFields(RecordData record, CTypeTranslator translator, Builder builder)
    {
        string GetTemplateType(ClangSharp.Type type)
        {
            // Only output a template type if it's not from our parsed headers.
            Decl? typeDecl = ClangTypeUtil.GetDeclaration(type);
            if (typeDecl != null) {
                if (builder.IsDeclPartOfSourceFile(typeDecl)) {
                    return "";
                }
            } 
            
            if (type is ElaboratedType) {
                return GetTemplateType(type.Desugar);
            }
            if (type is TemplateSpecializationType tst) {
                return string.Join(", ", tst.Args
                    .Select(a => GetArgString(a, translator))
                );
            }
            return "";
        }

        string GetType(ClangSharp.Type type)
        {
            // TODO: Perf.
            CTypeTranslator noSizeTranslator = new(translator);
            noSizeTranslator.IncludeConstantArraySizes = false;
            return noSizeTranslator.GetCType(type);
        }
        
        int GetSize(ClangSharp.Type type)
        {
            if (type.CanonicalType is ConstantArrayType at) {
                return (int)at.Size;
            }
            return 0;
        }

        IEnumerable<StructField> bases = record.BaseTypes
            .Select(bt => {
                string cName = translator.GetCType(bt);
                return new StructField {
                    Name = $"_base_{cName}",
                    Type = GetType(bt.ClangType),
                    Size = GetSize(bt.ClangType),
                    TemplateType = GetTemplateType(bt.ClangType)
                };
            });
        
        IEnumerable<StructField> fields = record.Fields
            .Select(f => new StructField {
                Name = f.Name,
                Type = GetType(f.Type.ClangType),
                Size = GetSize(f.Type.ClangType),
                TemplateType = GetTemplateType(f.Type.ClangType)
            });

        return bases.Concat(fields).ToList();
    }
}