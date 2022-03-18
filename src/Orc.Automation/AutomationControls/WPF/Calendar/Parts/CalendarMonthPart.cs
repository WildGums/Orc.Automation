namespace Orc.Automation.Controls;

using System;
using System.Linq;
using System.Windows.Automation;

public class CalendarMonthPart : AutomationControl
{
    private readonly CalendarMonthViewMap _map;

    public CalendarMonthPart(AutomationElement element)
        : base(element)
    {
        _map = new CalendarMonthViewMap(element);
    }

    public void SelectMonth(int month)
    {
        var button = _map.MonthsButtons
            .SkipWhile(x => DateTime.Parse((string)x.Current.HelpText).Month != 1)
            .FirstOrDefault(x => DateTime.Parse(x.Current.HelpText).Month == month);

        button?.Invoke();
    }
}