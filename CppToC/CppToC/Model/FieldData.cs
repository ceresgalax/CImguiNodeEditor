namespace CppToC.Model;

public class FieldData(string name, TypeRef type)
{
    public string Name = name;
    public TypeRef Type = type;
}