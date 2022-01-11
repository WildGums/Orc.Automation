namespace Orc.Automation.Tests.StyleAsserters;

using System.Windows.Media;
using Controls;
using NUnit.Framework;

public class ButtonThemeAssert : FrameworkElementAssert<ButtonThemeAssert, Button>
{
    public override void Theme(Button element)
    {
        base.Theme(element);

        ButtonThemeColorAssert.VerifyThemeColors(element);
    }
}
