namespace Orc.Automation.Tests.StyleAsserters;

using System.Windows.Automation;
using Controls;

public class ButtonThemeMap : AutomationBase
{
   // private Border _chrome;

    public ButtonThemeMap(AutomationElement button)
        : base(button)
    {
    }

    public Border Chrome => By.Name().Part<Border>();
    public Text Text => By.One<Text>();
}
