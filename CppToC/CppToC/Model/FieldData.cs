namespace CppToC.Model;

public class FieldData(string name, TypeRef type, bool isPublic)
{
    public string Name = name;
    public TypeRef Type = type;
    public bool IsPublic = isPublic;
}