using ClangSharp;
using ClangSharp.Interop;
using CppToC.Model;
using static ClangSharp.Interop.CXTypeKind;

namespace CppToC;

public static class CSourceGenerator
{
    public static void Generate(TextWriter writer, Builder builder, List<string> sourceHeaderFileNames, string headerFileName)
    {
        writer.WriteLine("#define FOR_WRAPPER_IMPL 1");
        foreach (string sourceHeaderFileName in sourceHeaderFileNames) {
            writer.WriteLine($"#include \"{sourceHeaderFileName}\"");   
        }
        writer.WriteLine($"#include \"{headerFileName}\"");
        writer.WriteLine("#include <utility> // std::move ");
        writer.WriteLine();
        writer.WriteLine("extern \"C\" {");

        CTypeTranslator cTypeTranslator = new();
        
        // Function impls
        foreach (FunctionData data in builder.Functions) {
            writer.WriteLine($"{cTypeTranslator.GetCFunctionLine(data, selfOf: null)} {{");
            writer.Write("\t");
            
            (string prefix, string postfix) = GetExpressionWrapper(data.ReturnType?.ClangType, cTypeTranslator);
            writer.Write(prefix);
            
            string cppSpelling = CUtil.GetCppFunctionSpelling(data);
            writer.Write($"{cppSpelling}(");

            for (int i = 0, ilen = data.Parameters.Count; i < ilen; ++i) {
                writer.Write(GetParameterPass(data.Parameters[i]));
                if (i + 1 < ilen) {
                    writer.Write(", ");
                }
            }
            writer.Write(")");
            writer.Write(postfix);
            writer.WriteLine(";");
            writer.WriteLine("}");
            writer.WriteLine();
        }
        
        // Method impls
        RecordMethodOwner recordMethodOwner = new(new RecordData());
        foreach (RecordData data in builder.Records) {
            recordMethodOwner.SetRecord(data);
            foreach (FunctionData function in data.Methods) {
                WriteMethod(writer, function, recordMethodOwner, cTypeTranslator);
            }
        }

        InstantiatedTemplateRecordMethodOwner itrMethodOwner = new();
        foreach (TemplateRecordData templateRecordData in builder.TemplateRecords.Values) {
            foreach (TemplateArgumentSet set in templateRecordData.Instantiations) {
                itrMethodOwner.Set(templateRecordData.Inner, set);
                cTypeTranslator.PushTemplateArgumentSet(set);
                
                foreach (FunctionData function in templateRecordData.Inner.Methods) {
                    WriteMethod(writer, function, itrMethodOwner, cTypeTranslator);    
                }
                
                cTypeTranslator.PopTemplateArgumentSet();
            }
        }
        
        writer.WriteLine("} // extern \"C\"");
    }

    private static (string prefix, string postfix) GetExpressionWrapper(ClangSharp.Type? returnType, CTypeTranslator translator)
    {
        if (returnType == null || returnType.Kind == CXType_Void) {
            return ("", "");
        }

        CXTypeKind rvKind = returnType.Kind;
        
        if (rvKind == CXType_Elaborated) {
            ElaboratedType et = (ElaboratedType)returnType;
            return GetExpressionWrapper(et.CanonicalType, translator);
        }
        
        bool shouldPassReturnValueAsOutParameter = translator.ShouldPassReturnValueAsOutParameter(returnType);
        bool isReturnValuePointer = rvKind is CXType_LValueReference or CXType_RValueReference;

        if (shouldPassReturnValueAsOutParameter) {
            string prefix = "*pOut = ";
            if (isReturnValuePointer) {
                prefix += "*";
            }
            
            return (prefix, "");
        }
        
        string cReturnType = translator.GetCType(returnType);
        
        if (isReturnValuePointer) {
            return ("return &", "");
        }

        if (cReturnType == "void*") {
            return ($"return reinterpret_cast<{cReturnType}>(", ")");
        }
        
        return ("return ", "");
    }

    private static string GetParameterPass(ParameterData parameter)
    {
        CXTypeKind kind = parameter.Type.ClangType.Kind;
        if (kind == CXType_LValueReference) {
            return "*" + parameter.Name;
        }
        if (kind == CXType_RValueReference) {
            return $"std::move(*{parameter.Name})";
        }
        return parameter.Name;
    }

    private static void WriteMethod(TextWriter writer, FunctionData function, IMethodOwner selfOf, CTypeTranslator translator)
    {
        writer.WriteLine($"{translator.GetCFunctionLine(function, selfOf)} {{");
        writer.Write("\t");

        (string prefix, string postfix) = GetExpressionWrapper(function.ReturnType?.ClangType, translator);
        writer.Write(prefix);

        string cppFuncSpelling = CUtil.GetCppFunctionSpelling(function); 
        writer.Write($"__self->{cppFuncSpelling}(");
                
        for (int i = 0, ilen = function.Parameters.Count; i < ilen; ++i) {
            writer.Write(GetParameterPass(function.Parameters[i]));
            if (i + 1 < ilen) {
                writer.Write(", ");
            }
        }
        writer.Write(")");
        writer.Write(postfix);
        writer.WriteLine(";");
        writer.WriteLine("}");
        writer.WriteLine();
    }
}