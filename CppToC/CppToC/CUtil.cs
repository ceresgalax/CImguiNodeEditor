using System.ComponentModel;
using System.Text;
using ClangSharp;
using ClangSharp.Interop;
using CppToC.Model;

namespace CppToC;

public static class CUtil
{
    public static string GetNamespacePrefix(string[] ns)
    {
        return string.Join("", ns);
    }

    public static string GetNamespacedName(string[] ns, string name)
    {
        string prefix = GetNamespacePrefix(ns);
        if (string.IsNullOrEmpty(prefix)) {
            return name;
        }
        return $"{prefix}_{name}";
    }
    
    public static string GetNamespacedName(NamedDecl decl)
    {
        return GetNamespacedName(GetNsFromCursor(decl), decl.Name);
    }
    
    public static string[] GetNsFromCursor(Cursor? cursor)
    {
        if (cursor == null) {
            return Array.Empty<string>();
        }
        
        Stack<string> parts = new();
        Cursor? lexicalParent = cursor.LexicalParentCursor;
        while (lexicalParent != null) {
            if (lexicalParent.CursorKind == CXCursorKind.CXCursor_TranslationUnit) {
                break;
            }
            parts.Push(lexicalParent.Spelling);
            lexicalParent = lexicalParent.LexicalParentCursor;
        }
        return parts.Where(s => !string.IsNullOrEmpty(s)).ToArray();
    }

    public static string GetOperatorOverloadName(CX_OverloadedOperatorKind kind)
    {
        return Enum.GetName(kind)?.Replace("CX_OO_", "").ToLowerInvariant() ?? throw new InvalidEnumArgumentException(nameof(kind), (int)kind, typeof(CX_OverloadedOperatorKind));
    }

    public static string GetRecordCppSpelling(RecordData record)
    {
        return GetRecordCppSpelling(record.Namespace, record.Name, Array.Empty<TemplateArgument>());
    }
    
    public static string GetRecordCppSpelling(string[] ns, string name, IReadOnlyList<TemplateArgument> templateArgs)
    {
        StringBuilder builder = new();
        
        if (ns.Length > 0) {
            foreach (string part in ns) {
                builder.Append(part);
                builder.Append("::");
            }
        }

        builder.Append(name);
        
        if (templateArgs.Count > 0) {
            builder.Append('<');

            for (int i = 0, ilen = templateArgs.Count; i < ilen; ++i) {
                TemplateArgument arg = templateArgs[i];
                builder.Append(GetNamespacedCppName(GetNsFromCursor(arg.AsType.Declaration), arg.AsType.AsString));
                if (i + 1 < ilen) {
                    builder.Append(", ");
                }
            }
            builder.Append('>');
        }

        return builder.ToString();
    }

    public static string GetCppFunctionSpelling(FunctionData data)
    {
        StringBuilder builder = new();
        builder.Append(GetNamespacedCppName(data.Namespace, data.Name));
        
        if (data.TemplateArgs.Length > 0) {
            builder.Append('<');

            for (int i = 0, ilen = data.TemplateArgs.Length; i < ilen; ++i) {
                TemplateArgument arg = data.TemplateArgs[i];
                builder.Append(arg.AsType.AsString);
                if (i + 1 < ilen) {
                    builder.Append(", ");
                }
            }
            builder.Append('>');
        }

        return builder.ToString();
    }
  
    public static string GetNamespacedCppName(string[] ns, string name)
    {
        StringBuilder builder = new();
        
        if (ns.Length > 0) {
            foreach (string part in ns) {
                builder.Append(part);
                builder.Append("::");
            }
        }
        
        builder.Append(name);
        return builder.ToString();
    }
    
}