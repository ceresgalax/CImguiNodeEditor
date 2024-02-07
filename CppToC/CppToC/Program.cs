// See https://aka.ms/new-console-template for more information

using System.Net;
using ClangSharp;
using ClangSharp.Interop;
using CppToC;
using CppToC.Generators;
using CppToC.Model;

string pwd = Directory.GetCurrentDirectory();

string sourceFile = $"{pwd}/imgui-node-editor/imgui_node_editor.h";
string includeDirectory = $"{pwd}/imgui";
string headerOutputPath = $"{pwd}/cimgui_node_editor.h";
string sourceOutputPath = $"{pwd}/cimgui_node_editor.cpp";
string cimguiDefsOutputPath = $"{pwd}/definitions.json";
string cimguiStructsAndEnumsOutputPath = $"{pwd}/structs_and_enums.json";


string clangVersion;
try
{
    clangVersion = clang.getClangVersion().ToString();
    Console.WriteLine($"Clang {clangVersion}"); 
}
catch
{
    Console.WriteLine();
    Console.WriteLine("*****IMPORTANT*****");
    Console.WriteLine($"Failed to resolve libClang.");
    Console.WriteLine("If you are running as a dotnet tool, you may need to manually copy the appropriate DLLs from NuGet due to limitations in the dotnet tool support. Please see https://github.com/dotnet/clangsharp for more details.");
    Console.WriteLine("*****IMPORTANT*****");
    Console.WriteLine();
    throw;
}


string[] commandLineArgs = [
    "--language=c++",
    "-Wno-pragma-once-outside-header",
    "--include-directory=" + includeDirectory
];

CXIndex cxIndex = CXIndex.Create();
// TODO: WHAT ARE THE CORRECT OPTIONS HERE? (If we want to generate for bindings for macros, we may need an option to enable detailed preprocessor record)
CXErrorCode parseError = CXTranslationUnit.TryParse(cxIndex, sourceFile, commandLineArgs: commandLineArgs, unsavedFiles: Array.Empty<CXUnsavedFile>(), options: CXTranslationUnit_Flags.CXTranslationUnit_None, out CXTranslationUnit cxTranslationUnit);
if (parseError != CXErrorCode.CXError_Success) {
    Console.WriteLine($"Error: Parsing failed due to '{parseError}'.");
    return;
}

Console.WriteLine($"Num diagnostics: {cxTranslationUnit.NumDiagnostics}");
for (uint i = 0, ilen = cxTranslationUnit.NumDiagnostics; i < ilen; ++i) {
    Console.WriteLine(cxTranslationUnit.GetDiagnostic(i).ToString());
}

using TranslationUnit translationUnit = TranslationUnit.GetOrCreate(cxTranslationUnit);

Builder builder = new();
builder.AddSourcePath(sourceFile);
CursorVisitor.Visit(translationUnit.TranslationUnitDecl, builder);

OverloadUtil.ProcessOverloads(builder);

{
    Console.WriteLine("Generating Header...");
    using StreamWriter writer = File.CreateText(headerOutputPath);
    CHeaderGenerator.Generate(writer, builder);
}

{
    Console.WriteLine("Generator Source...");
    using StreamWriter writer = File.CreateText(sourceOutputPath);
    CSourceGenerator.Generate(writer, builder, Path.GetFileName(headerOutputPath),  Path.GetFileName(sourceFile));
}

{
    Console.WriteLine("Writing cimgui-style definitions.json...");
    using StreamWriter writer = File.CreateText(cimguiDefsOutputPath);
    CimguiDefinitionsGenerator.Generate(writer, builder);
}

{
    Console.WriteLine("Writing cimgui-style structs_and_enums.json...");
    using StreamWriter writer = File.CreateText(cimguiStructsAndEnumsOutputPath);
    CImguiStructsAndEnumsGenerator.Generate(writer, builder);
}

Console.WriteLine("Done!");
