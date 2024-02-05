using ClangSharp;
using ClangSharp.Interop;
using CppToC.Model;
using static ClangSharp.Interop.CXTypeKind;

namespace CppToC;

public static class CSourceGenerator
{
    public static void Generate(TextWriter writer, Builder builder, string headerFileName, string sourceHeaderFileName)
    {
        writer.WriteLine("#define FOR_WRAPPER_IMPL 1");
        writer.WriteLine($"#include \"{sourceHeaderFileName}\" // This needs to be included first!");
        writer.WriteLine($"#include \"{headerFileName}\"");
        writer.WriteLine("#include <utility> // std::move ");
        writer.WriteLine();
        //writer.WriteLine("extern \"C\" {");
        
        // Output C struct ot CPP Record typedefs
        // foreach (RecordData data in builder.Records) {
        //     string recordCName = CUtil.GetNamespacedName(data.Namespace, data.Name, data.TemplateArgs);
        //     string cppName = CUtil.GetRecordSpelling(data);
        //     writer.WriteLine($"typedef {recordCName} {cppName};");
        // }
        
        // Function impls
        foreach (FunctionData data in builder.Functions) {
            writer.WriteLine($"{CUtil.GetCFunctionLine(data, selfOf: null)} {{");
            writer.Write("\t");
            
            (string prefix, string postfix) = GetExpressionWrapper(data.ReturnType?.ClangType);
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
        foreach (RecordData data in builder.Records) {
            foreach (FunctionData function in data.Methods) {
                writer.WriteLine($"{CUtil.GetCFunctionLine(function, selfOf: data)} {{");
                writer.Write("\t");

                (string prefix, string postfix) = GetExpressionWrapper(function.ReturnType?.ClangType);
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
        
        //writer.WriteLine("} // extern \"C\""); // end extern "C"
    }

    private static (string prefix, string postfix) GetExpressionWrapper(ClangSharp.Type? returnType)
    {
        if (returnType == null || returnType.Kind == CXType_Void) {
            return ("", "");
        }

        CXTypeKind rvKind = returnType.Kind;

        if (rvKind == CXType_Elaborated) {
            ElaboratedType et = (ElaboratedType)returnType;
            return GetExpressionWrapper(et.CanonicalType);
        }
        
        if (rvKind is CXType_LValueReference or CXType_RValueReference) {
            return ("return &", "");
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
}