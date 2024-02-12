using ClangSharp;
using ClangSharp.Interop;

namespace CppToC.Model;

public class TemplateArgumentSet : IEquatable<TemplateArgumentSet>
{
    public readonly string Str;
    public readonly IReadOnlyList<TemplateArgument> Args;
    
    public TemplateArgumentSet(IEnumerable<TemplateArgument> args)
    {
        TemplateArgument[] ownedArgs = args.ToArray();

        List<string> argStrings = new();
        for (int i = 0, ilen = ownedArgs.Length ; i <ilen; ++i) {
            TemplateArgument arg = ownedArgs[i];

            if (CUtil.TemplateArgumentStack.Count > 0) {
                if (arg.AsType is TemplateTypeParmType ttp) {
                    TemplateArgumentSet set = CUtil.TemplateArgumentStack[(int)ttp.Depth];
                    arg = set.Args[(int)ttp.Index];
                    ownedArgs[i] = arg;
                }
            }
            
            string valString = arg.Kind switch {
                CXTemplateArgumentKind.CXTemplateArgumentKind_Type => CUtil.GetCType(arg.AsType)
                , CXTemplateArgumentKind.CXTemplateArgumentKind_Integral => arg.AsIntegral.ToString()
                , _ => throw new ArgumentOutOfRangeException()
            };
            argStrings.Add($"{arg.Kind}_{valString}");
        }

        Str = string.Join(',', argStrings);
        Args = ownedArgs;
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
    public IReadOnlyList<NamedDecl> Parameters;
}