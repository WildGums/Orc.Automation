namespace Orc.Automation;

using System;
using System.Windows.Controls;

[ActiveAutomationModel]
public class CalendarModel : ControlModel
{
    public CalendarModel(AutomationElementAccessor accessor) 
        : base(accessor)
    {
    }

    public DateTime DisplayDate { get; set; }
    public DateTime? DisplayDateEnd { get; set; }
    public DateTime? DisplayDateStart { get; set; }
    public CalendarMode DisplayMode { get; set; }
    public DayOfWeek FirstDayOfWeek { get; set; }
    public DateTime? SelectedDate { get; set; }
    public CalendarSelectionMode SelectionMode { get; set; }
    public bool IsTodayHighlighted { get; set; }
}