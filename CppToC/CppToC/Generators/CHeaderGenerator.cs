﻿using System.Text;
using ClangSharp;
using ClangSharp.Interop;

namespace CppToC.Model;

public class CHeaderGenerator
{
    public static void Generate(TextWriter writer, Builder builder, string exportHeaderIncludePath, string exportMacro)
    {
        writer.WriteLine("#pragma once");
        writer.WriteLine($"#include \"{exportHeaderIncludePath}\"");
        writer.WriteLine("#if !FOR_WRAPPER_IMPL");
        writer.WriteLine();

        CTypeTranslator cTypeTranslator = new();
        
        // Output enums!
        foreach (EnumData data in builder.Enums) {
            string enumName = CUtil.GetNamespacedName(data.Namespace, data.Name);
            writer.WriteLine($"enum {enumName} {{");
            for (int i = 0, ilen = data.Constants.Count; i < ilen; ++i) {
                EnumConstant constant = data.Constants[i];
                string comma = i + 1 < ilen ? "," : "";
                writer.WriteLine($"\t{constant.Name} = {constant.Value}{comma}");
            }
            writer.WriteLine("};");
            writer.WriteLine();
        }
        
        // Output struct forward decls
        foreach (RecordData data in builder.Records) {
            string recordCName = CUtil.GetNamespacedName(data.Namespace, data.Name);
            writer.WriteLine($"struct {recordCName};");
            writer.WriteLine($"typedef struct {recordCName} {recordCName};");
        }
        foreach (TemplateRecordData templateRecordData in builder.TemplateRecords.Values) {
            foreach (TemplateArgumentSet set in templateRecordData.Instantiations) {
                RecordData data = templateRecordData.Inner;
                string recordCName = cTypeTranslator.GetNamespacedName(data.Namespace, data.Name, set.Args);
                writer.WriteLine($"struct {recordCName};");
                writer.WriteLine($"typedef struct {recordCName} {recordCName};");
            }
        }
        foreach (ForwardDeclaredRecordData data in builder.ForwardDeclaredRecords) {
            string recordCName = CUtil.GetNamespacedName(data.Namespace, data.Name);
            writer.WriteLine($"struct {recordCName};");
            writer.WriteLine($"typedef struct {recordCName} {recordCName};");
        }
        writer.WriteLine("");
        
        // Output structs!
        foreach (RecordData data in builder.Records) {
            string recordCName = cTypeTranslator.GetNamespacedName(data.Namespace, data.Name, Array.Empty<TemplateArgument>()); 
            WriteStructInner(writer, recordCName, data, cTypeTranslator);
        }

        foreach (TemplateRecordData templateRecordData in builder.TemplateRecords.Values) {
            foreach (TemplateArgumentSet set in templateRecordData.Instantiations) {
                cTypeTranslator.PushTemplateArgumentSet(set);
                
                RecordData data = templateRecordData.Inner;
                string recordCName = cTypeTranslator.GetNamespacedName(data.Namespace, data.Name, set.Args);

                WriteStructInner(writer, recordCName, data, cTypeTranslator);
                
                cTypeTranslator.PopTemplateArgumentSet();
            }
        }
        
        writer.WriteLine("#else");
        
        // Output c++ typedefs for wrapper impl code

        foreach (EnumData data in builder.Enums) {
            string enumCName = CUtil.GetNamespacedName(data.Namespace, data.Name);
            string enumCppName = CUtil.GetNamespacedCppName(data.Namespace, data.Name);
            writer.WriteLine($"typedef {enumCppName} {enumCName};");
        }
        
        foreach (RecordData data in builder.Records) {
            string recordCName = cTypeTranslator.GetNamespacedName(data.Namespace, data.Name, Array.Empty<TemplateArgument>());
            string recordCppName = CUtil.GetRecordCppSpelling(data);
            writer.WriteLine($"typedef {recordCppName} {recordCName};");
        }
        foreach (TemplateRecordData templateRecordData in builder.TemplateRecords.Values) {
            foreach (TemplateArgumentSet set in templateRecordData.Instantiations) {
                cTypeTranslator.PushTemplateArgumentSet(set);
                
                RecordData data = templateRecordData.Inner;
                string recordCName = cTypeTranslator.GetNamespacedName(data.Namespace, data.Name, set.Args);
                string recordCppName = CUtil.GetRecordCppSpelling(data.Namespace, data.Name, set.Args);
                writer.WriteLine($"typedef {recordCppName} {recordCName};");
                
                cTypeTranslator.PopTemplateArgumentSet();
            }
        }

        foreach (ForwardDeclaredRecordData data in builder.ForwardDeclaredRecords) {
            string recordCName = cTypeTranslator.GetNamespacedName(data.Namespace, data.Name, Array.Empty<TemplateArgument>());
            string recordCppName = CUtil.GetRecordCppSpelling(data.Namespace, data.Name, Array.Empty<TemplateArgument>());
            writer.WriteLine($"typedef {recordCppName} {recordCName};");
        }
        
        writer.WriteLine("#endif // !FOR_WRAPPER_IMPL");
        writer.WriteLine();
        
        
        writer.WriteLine("#ifdef __cplusplus");
        writer.WriteLine("extern \"C\" {");
        writer.WriteLine("#endif // __cplusplus");
        
        // Output functions!
        foreach (FunctionData data in builder.Functions) {
            writer.WriteLine($"{exportMacro} {cTypeTranslator.GetCFunctionLine(data, selfOf: null)};");
        }
        
        // Output methods
        RecordMethodOwner recordMethodOwner = new(new RecordData());
        foreach (RecordData record in builder.Records) {
            recordMethodOwner.SetRecord(record);
            foreach (FunctionData function in record.Methods) {
                writer.WriteLine($"{exportMacro} {cTypeTranslator.GetCFunctionLine(function, recordMethodOwner)};");
            }
        }
        InstantiatedTemplateRecordMethodOwner itrMethodOwner = new();
        foreach (TemplateRecordData templateRecordData in builder.TemplateRecords.Values) {
            foreach (TemplateArgumentSet set in templateRecordData.Instantiations) {
                itrMethodOwner.Set(templateRecordData.Inner, set);
                cTypeTranslator.PushTemplateArgumentSet(set);
                foreach (FunctionData function in templateRecordData.Inner.Methods) {
                    writer.WriteLine($"{exportMacro} {cTypeTranslator.GetCFunctionLine(function, itrMethodOwner)};");
                }
                cTypeTranslator.PopTemplateArgumentSet();
            }
        }
        
        writer.WriteLine("#ifdef __cplusplus");
        writer.WriteLine("} // extern \"C\"");
        writer.WriteLine("#endif // __cplusplus");
    }

    private static void WriteStructInner(TextWriter writer, string cName, RecordData data, CTypeTranslator translator)
    {
        writer.WriteLine($"struct {cName} {{");

        if (data.BaseTypes.Count > 0) {
            foreach (TypeRef baseType in data.BaseTypes) {
                // TODO: If there are multiple bases, is the order in the AST guaranteed to be in the order of how
                // the derived fields will be laid out in C++?
                string baseCType = translator.GetCType(baseType);
                writer.WriteLine($"\t{baseCType,-20} _base_{baseCType}");
            }
            writer.WriteLine();    
        }
            
        foreach (FieldData field in data.Fields) {
            writer.WriteLine($"\t{translator.GetCType(field.Type),-20} {field.Name};");
        }
            
        writer.WriteLine("};");
        writer.WriteLine();
    }

}