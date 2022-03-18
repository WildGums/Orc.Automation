namespace Orc.Automation.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Globalization;
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
                .SkipWhile(x => DateTime.Parse(x.Current.HelpText).Month != 1)
                .FirstOrDefault(x => DateTime.Parse(x.Current.HelpText).Month == month);

            button?.Invoke();
        }
    }

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
                .FirstOrDefault(x => DateTime.Parse(x.Current.HelpText).Year == year);

            button?.Invoke();
        }
    }

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

    public class CalendarDayViewMap : AutomationBase
    {
        public CalendarDayViewMap(AutomationElement element)
            : base(element)
        {
        }

        public IList<AutomationElement> DayButtons => By.ClassName("CalendarDayButton").Many();
    }

    public class CalendarMonthViewMap : AutomationBase
    {
        public CalendarMonthViewMap(AutomationElement element)
            : base(element)
        {
        }

        public IList<AutomationElement> MonthsButtons => By.ClassName("CalendarButton").Many();
    }

    public class CalendarYearViewMap : AutomationBase
    {
        public CalendarYearViewMap(AutomationElement element)
            : base(element)
        {
        }

        public IList<AutomationElement> YearButtons => By.ClassName("CalendarButton").Many();
    }


    [AutomatedControl(ControlTypeName = nameof(ControlType.Calendar))]
    public class Calendar : FrameworkElement
    {
        private readonly CalendarMap _map;

        public Calendar(AutomationElement element) 
            : base(element, ControlType.Calendar)
        {
            _map = new CalendarMap(element);
        }

        public DateTime? SelectedDate
        {
            get => GetSelectedDate();
            set => SetSelectedDate(value);
        }

        private DateTime? GetSelectedDate()
        {
            var selectedElements = Element.GetSelection();
            foreach (var selectedElement in selectedElements)
            {
                var name = selectedElement.Current.Name;

                try
                {
                    var dateTime = DateTime.Parse(name, CultureInfo.CurrentCulture);
                    return dateTime;
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }

        private void SetSelectedDate(DateTime? date)
        {
            if (date is null)
            {
                var selection = Element.GetSelection();
                foreach (var automationElement in selection)
                {
                    automationElement.TrySetSelection(false);
                }

                return;
            }

            var dateValue = date.Value;

            BringYearIntoView(dateValue.Year);

            Wait.UntilResponsive();

            _map.YearPart.SelectYear(dateValue.Year);

            Wait.UntilResponsive();

            _map.MonthPart.SelectMonth(dateValue.Month);

            Wait.UntilResponsive();

            _map.DaysPart.SelectDay(dateValue.Day);
        }

        private void BringYearIntoView(int year)
        {
            Element.TryRunPatternFunc<MultipleViewPattern>(x =>
            {
                var supportedViews = x.Current.GetSupportedViews();
                if (supportedViews.Length >= 3)
                {
                    x.SetCurrentView(supportedViews[2]);
                }
            });

            var headerButton = _map.HeaderButton;
            var content = headerButton.Content;

            var items = content.Split('-');
            if (!int.TryParse(items[0], out var minYear))
            {
                return;
            }

            if (!int.TryParse(items[1], out var maxYear))
            {
                return;
            }

            if (year < minYear)
            {
                for (var child = _map.PreviousButton; year < minYear && child.Click(); minYear = int.Parse(headerButton.Content.Split('-')[0]))
                {

                }
            }
            else if (year > maxYear)
            {
                for (var child = _map.NextButton; year > maxYear && child.Click(); maxYear = int.Parse(headerButton.Content.Split('-')[1]))
                {

                }
            }
        }
    }
}
