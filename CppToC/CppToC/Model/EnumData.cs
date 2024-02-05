namespace CppToC.Model;

public class EnumConstant
{
    public string Name;
    public long Value;

}

public class EnumData
{
    public string[] Namespace;
    public string Name;
    public List<EnumConstant> Constants = new ();
}