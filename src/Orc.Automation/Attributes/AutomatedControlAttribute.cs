namespace Orc.Automation;

public class AutomatedControlAttribute : ControlAttribute
{
    public override string ClassName
    {
        get =>  $"{base.ClassName}{NameConventions.ActiveModelControlClassNameSuffix}";
        set => base.ClassName = value;
    }
}