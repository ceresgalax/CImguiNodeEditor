using CppToC.Model;

namespace CppToC;

public static class OverloadUtil
{
    public static void ProcessOverloads(Builder builder)
    {
        ProcessOverloads(builder.Functions);
        foreach (RecordData record in builder.Records) {
            ProcessOverloads(record.Methods);    
        }
    }

    public static void ProcessOverloads(IEnumerable<FunctionData> functions)
    {
        Dictionary<string, int> overloadIndices = new();

        foreach (FunctionData func in functions) {
            overloadIndices.TryGetValue(func.Name, out int index);
            func.OverloadIndex = index;
            overloadIndices[func.Name] = index + 1;
        }
    }
}