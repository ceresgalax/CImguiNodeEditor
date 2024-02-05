using ClangSharp;
using ClangSharp.Interop;

namespace CppToC.Model;

public class TemplateArgumentSet : IEquatable<TemplateArgumentSet>
{
    public readonly string Str;
    public readonly IReadOnlyList<TemplateArgument> Args;
    
    public TemplateArgumentSet(IEnumerable<TemplateArgument> args)
    {
        Args = args.ToArray().AsReadOnly();

        List<string> argStrings = new();
        foreach (TemplateArgument arg in Args) {
            string valString = arg.Kind switch {
                CXTemplateArgumentKind.CXTemplateArgumentKind_Type => CUtil.GetCType(arg.AsType)
                , CXTemplateArgumentKind.CXTemplateArgumentKind_Integral => arg.AsIntegral.ToString()
                , _ => throw new ArgumentOutOfRangeException()
            };
            argStrings.Add($"{arg.Kind}_{valString}");
        }

        Str = string.Join(',', argStrings);
    }
    
    public bool Equals(TemplateArgumentSet? other)
    {
        if (other == null) {
            return false;
        }
        return Str == other.Str;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((TemplateArgumentSet)obj);
    }

    public override int GetHashCode()
    {
        return Str.GetHashCode();
    }
}

public class TemplateRecordData
{
    public RecordData Inner = new();
    public HashSet<TemplateArgumentSet> Instantiations = new();
    public IReadOnlyList<TemplateArgument> Args;
}