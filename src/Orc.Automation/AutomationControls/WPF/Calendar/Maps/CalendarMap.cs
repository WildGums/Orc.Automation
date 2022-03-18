namespace Orc.Automation.Controls;

using System.Windows.Automation;

public class CalendarMap : AutomationBase
{
    public CalendarMap(AutomationElement element)
        : base(element)
    {
    }

    public Button HeaderButton => By.Id("PART_HeaderButton").One<Button>();
    public Button PreviousButton => By.Id("PART_PreviousButton").One<Button>();
    public Button NextButton => By.Id("PART_NextButton").One<Button>();

    public CalendarDaysPart DaysPart => Element.InvokeInView(0, () => new CalendarDaysPart(Element));
    public CalendarMonthPart MonthPart => Element.InvokeInView(1, () => new CalendarMonthPart(Element));
    public CalendarYearPart YearPart => Element.InvokeInView(2, () => new CalendarYearPart(Element));
}