namespace Orc.Automation
{
    using System.Windows.Automation;
    using System.Windows.Input;
    using Catel;

    public static partial class AutomationElementExtensions
    {
        public static void MouseClick(this AutomationElement element, MouseButton mouseButton = MouseButton.Left)
        {
            Argument.IsNotNull(() => element);
            
            element.MouseHover();
            MouseInput.Click(mouseButton);
        }

        public static void MouseHover(this AutomationElement element)
        {
            Argument.IsNotNull(() => element);

            var rect = element.Current.BoundingRectangle;

            MouseInput.MoveTo(rect.GetClickablePoint());
        }

        public static void MouseOut(this AutomationElement element)
        {
            Argument.IsNotNull(() => element);

            var rect = element.Current.BoundingRectangle;

            MouseInput.MoveTo(rect.GetPointOut());
        }
    }
}
