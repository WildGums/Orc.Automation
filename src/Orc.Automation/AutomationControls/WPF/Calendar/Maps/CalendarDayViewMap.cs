namespace Orc.Automation.Controls;

using System.Collections.Generic;
using System.Windows.Automation;

public class CalendarDayViewMap : AutomationBase
{
    public CalendarDayViewMap(AutomationElement element)
        : base(element)
    {
    }

    public IList<AutomationElement> DayButtons => By.ClassName("CalendarDayButton").Many();
}