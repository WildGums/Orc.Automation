namespace Orc.Automation.Controls;

using System;
using System.Linq;
using System.Windows.Automation;

public class CalendarDaysPart : AutomationControl
{
    private readonly CalendarDayViewMap _map;

    public CalendarDaysPart(AutomationElement element)
        : base(element)
    {
        _map = new CalendarDayViewMap(element);
    }

    public void SelectDay(int day)
    {
        var button = _map.DayButtons
            .SkipWhile(x => DateTime.Parse(x.Current.HelpText).Day != 1)
            .FirstOrDefault(x => DateTime.Parse(x.Current.HelpText).Day == day);

        button?.Invoke();
    }
}