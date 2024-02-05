using CppToC.Model;

namespace CppToC;

public static class OverloadUtil
{
    public static void ProcessOverloads(Builder builder)
    {
        foreach (RecordData record in builder.Records) {
            ProcessOverloads(record);
        }
    }

    public static void ProcessOverloads(RecordData record)
    {
        Dictionary<string, int> overloadIndices = new();

        foreach (FunctionData method in record.Methods) {
            overloadIndices.TryGetValue(method.Name, out int index);
            method.OverloadIndex = index;
            overloadIndices[method.Name] = index + 1;
        }
    }
}