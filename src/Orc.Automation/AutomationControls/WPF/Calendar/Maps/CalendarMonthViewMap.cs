namespace Orc.Automation.Controls;

using System.Collections.Generic;
using System.Windows.Automation;

public class CalendarMonthViewMap : AutomationBase
{
    public CalendarMonthViewMap(AutomationElement element)
        : base(element)
    {
    }

    public IList<AutomationElement> MonthsButtons => By.ClassName("CalendarButton").Many();
}