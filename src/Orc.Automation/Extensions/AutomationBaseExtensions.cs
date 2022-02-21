namespace Orc.Automation
{
    public static class AutomationBaseExtensions
    {
        public static bool IsVisible(this AutomationBase element)
        {
            //Automation can't find element if it's not visible, so no checks for null
            if (element is null)
            {
                return false;
            }

            return element.Element.IsVisible();
        }
    }
}
