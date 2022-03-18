namespace Orc.Automation.Controls;

using System.Collections.Generic;
using System.Windows.Automation;

public class CalendarYearViewMap : AutomationBase
{
    public CalendarYearViewMap(AutomationElement element)
        : base(element)
    {
    }

    public IList<AutomationElement> YearButtons => By.ClassName("CalendarButton").Many();
}