namespace Orc.Automation.Controls;

using System;
using System.Linq;
using System.Windows.Automation;

public class CalendarYearPart : AutomationControl
{
    private readonly CalendarYearViewMap _map;

    public CalendarYearPart(AutomationElement element)
        : base(element)
    {
        _map = new CalendarYearViewMap(element);
    }

    public void SelectYear(int year)
    {
        var button = _map.YearButtons
            .FirstOrDefault(x => DateTime.Parse((string)x.Current.HelpText).Year == year);

        button?.Invoke();
    }
}