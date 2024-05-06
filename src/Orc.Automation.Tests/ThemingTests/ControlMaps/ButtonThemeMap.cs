namespace Orc.Automation.Tests;

using System.Windows.Automation;
using Controls;

public class ButtonThemeMap : AutomationBase
{
    public ButtonThemeMap(AutomationElement button)
        : base(button)
    {
    }

    public Border Chrome => By.Name().Part<Border>();
    public Text Text => By.One<Text>();
}