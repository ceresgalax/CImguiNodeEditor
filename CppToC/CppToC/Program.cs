// See https://aka.ms/new-console-template for more information

using ClangSharp;
using ClangSharp.Interop;
using CppToC;
using CppToC.Generators;
using CppToC.Model;

string pwd = Directory.GetCurrentDirectory();

//string sourceFile = $"{pwd}/imgui-node-editor/imgui_node_editor.h";
List<string> includeDirectories = new() {
    $"{pwd}/imgui",
    $"{pwd}/imgui-node-editor"
};

string headerOutputPath = $"{pwd}/cimgui_node_editor.h";
string sourceOutputPath = $"{pwd}/cimgui_node_editor.cpp";
string cimguiDefsOutputPath = $"{pwd}/definitions.json";
string cimguiStructsAndEnumsOutputPath = $"{pwd}/structs_and_enums.json";

List<string> sourceFilePaths = new() {
    $"{pwd}/imgui-node-editor/imgui_node_editor.h",
    // $"{pwd}/imgui-node-editor/examples/blueprints-example/utilities/builders.h",
    // $"{pwd}/imgui-node-editor/examples/blueprints-example/utilities/drawing.h",
    // $"{pwd}/imgui-node-editor/examples/blueprints-example/utilities/widgets.h",
};


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


List<string> commandLineArgs = [
    "--language=c++",
    "-Wno-pragma-once-outside-header",
];
commandLineArgs.AddRange(includeDirectories.Select(x => $"-I{x}"));

CXIndex cxIndex = CXIndex.Create();

Builder builder = new();
builder.SetSourcePaths(sourceFilePaths);

bool hadFailure = false;

List<TranslationUnit> translationUnits = new();

foreach (string sourceFile in sourceFilePaths) {
    // Note: If we want to generate for bindings for macros, we may need an option to enable detailed preprocessor record.
    CXErrorCode parseError = CXTranslationUnit.TryParse(
        cxIndex,
        sourceFile,
        commandLineArgs: commandLineArgs.ToArray(),
        unsavedFiles: Array.Empty<CXUnsavedFile>(),
        options: CXTranslationUnit_Flags.CXTranslationUnit_None,
        out CXTranslationUnit cxTranslationUnit
    );
    if (parseError != CXErrorCode.CXError_Success) {
        Console.WriteLine($"Error: Parsing {sourceFile} failed due to '{parseError}'.");
        hadFailure = true;
        continue;
    }
    
    Console.WriteLine($"Num diagnostics for source file {sourceFile}: {cxTranslationUnit.NumDiagnostics}");
    for (uint i = 0, ilen = cxTranslationUnit.NumDiagnostics; i < ilen; ++i) {
        CXDiagnostic diagnostic = cxTranslationUnit.GetDiagnostic(i);
        Console.WriteLine($"{diagnostic.Location}: {diagnostic.ToString()}");
    }
    
    TranslationUnit translationUnit = TranslationUnit.GetOrCreate(cxTranslationUnit);
    translationUnits.Add(translationUnit);
}

if (hadFailure) {
    return;
}


foreach (TranslationUnit translationUnit in translationUnits) {
    CursorVisitor.Visit(translationUnit.TranslationUnitDecl, builder);    
}

OverloadUtil.ProcessOverloads(builder);

{
    Console.WriteLine("Generating Header...");
    using StreamWriter writer = File.CreateText(headerOutputPath);
    CHeaderGenerator.Generate(writer, builder, "cimgui_node_editor_export.h", "CIMGUI_NODE_EDITOR_EXPORT");
}

{
    Console.WriteLine("Generator Source...");
    using StreamWriter writer = File.CreateText(sourceOutputPath);
    List<string> headerFileNames = sourceFilePaths
        .Select(p => Path.GetRelativePath($"{pwd}/imgui-node-editor", p))
        .ToList();
    CSourceGenerator.Generate(writer, builder, headerFileNames, Path.GetFileName(headerOutputPath));
}

string[] cimguiOmittedNsPrefix = ["ax", "NodeEditor"];

{
    Console.WriteLine("Writing cimgui-style definitions.json...");
    using StreamWriter writer = File.CreateText(cimguiDefsOutputPath);
    CimguiDefinitionsGenerator.Generate(writer, builder, cimguiOmittedNsPrefix);
}

{
    Console.WriteLine("Writing cimgui-style structs_and_enums.json...");
    using StreamWriter writer = File.CreateText(cimguiStructsAndEnumsOutputPath);
    CImguiStructsAndEnumsGenerator.Generate(writer, builder, cimguiOmittedNsPrefix);
}

Console.WriteLine("Done!");
