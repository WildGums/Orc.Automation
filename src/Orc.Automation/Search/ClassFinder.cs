namespace Orc.Automation;

public class ClassFinder : ConditionalPartFinderBase
{
    public static ClassFinder Create(string className) => new()
    {
        ClassName = className
    };

    public string ClassName { get; set; }

    protected override bool IsMatch(object descendant)
    {
        return Equals(descendant.GetType().FullName, ClassName);
    }
}