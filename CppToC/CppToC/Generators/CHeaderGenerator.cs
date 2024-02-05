﻿using System.Text;
using ClangSharp;
using ClangSharp.Interop;

namespace CppToC.Model;

public class CHeaderGenerator
{
    public static void Generate(TextWriter writer, Builder builder)
    {
        writer.WriteLine("#pragma once");
        writer.WriteLine("#if !FOR_WRAPPER_IMPL");
        writer.WriteLine();
        
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
        foreach (ForwardDeclaredRecordData data in builder.ForwardDeclaredRecords) {
            string recordCName = CUtil.GetNamespacedName(data.Namespace, data.Name);
            writer.WriteLine($"struct {recordCName};");
            writer.WriteLine($"typedef struct {recordCName} {recordCName};");
        }
        writer.WriteLine("");
        
        // Output structs!
        foreach (RecordData data in builder.Records) {
            string recordCName = CUtil.GetNamespacedName(data.Namespace, data.Name, data.TemplateArgs); 

            writer.WriteLine($"struct {recordCName} {{");

            if (data.BaseTypes.Count > 0) {
                foreach (TypeRef baseType in data.BaseTypes) {
                    // TODO: If there are multiple bases, is the order in the AST guaranteed to be in the order of how
                    // the derived fields will be laid out in C++?
                    string baseCType = CUtil.GetCType(baseType);
                    writer.WriteLine($"\t{baseCType,-20} _base_{baseCType}");
                }
                writer.WriteLine();    
            }
            
            foreach (FieldData field in data.Fields) {
                writer.WriteLine($"\t{CUtil.GetCType(field.Type),-20} {field.Name};");
            }
            
            writer.WriteLine("};");
            writer.WriteLine();
        }
        
        writer.WriteLine("#else");
        
        // Output c++ typedefs for wrapper impl code

        foreach (EnumData data in builder.Enums) {
            string enumCName = CUtil.GetNamespacedName(data.Namespace, data.Name);
            string enumCppName = CUtil.GetNamespacedCppName(data.Namespace, data.Name);
            writer.WriteLine($"typedef {enumCppName} {enumCName};");
        }
        
        foreach (RecordData data in builder.Records) {
            string recordCName = CUtil.GetNamespacedName(data.Namespace, data.Name, data.TemplateArgs);
            string recordCppName = CUtil.GetRecordCppSpelling(data);
            writer.WriteLine($"typedef {recordCppName} {recordCName};");
        }
        
        foreach (ForwardDeclaredRecordData data in builder.ForwardDeclaredRecords) {
            string recordCName = CUtil.GetNamespacedName(data.Namespace, data.Name, Array.Empty<TemplateArgument>());
            string recordCppName = CUtil.GetRecordCppSpelling(data.Namespace, data.Name, Array.Empty<TemplateArgument>());
            writer.WriteLine($"typedef {recordCppName} {recordCName};");
        }
        
        writer.WriteLine("#endif // !FOR_WRAPPER_IMPL");
        writer.WriteLine();
        
        
        // Output functions!
        foreach (FunctionData data in builder.Functions) {
            writer.WriteLine($"{CUtil.GetCFunctionLine(data, selfOf: null)};");
        }
        
        // Output methods
        foreach (RecordData record in builder.Records) {
            foreach (FunctionData function in record.Methods) {
                writer.WriteLine($"{CUtil.GetCFunctionLine(function, record)};");
            }
        }
    }

}