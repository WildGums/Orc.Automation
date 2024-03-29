﻿namespace Orc.Automation.Controls;

using System;
using System.Globalization;
using System.Windows.Automation;

[Control(ControlTypeName = nameof(ControlType.Calendar))]
public class Calendar : FrameworkElement<CalendarModel, CalendarMap>
{
    public Calendar(AutomationElement element) 
        : base(element, ControlType.Calendar)
    {
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

        var map = Map;

        map.YearPart.SelectYear(dateValue.Year);

        Wait.UntilResponsive();

        map.MonthPart.SelectMonth(dateValue.Month);

        Wait.UntilResponsive();

        map.DaysPart.SelectDay(dateValue.Day);
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

        var map = Map;

        var headerButton = map.HeaderButton;
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
            for (var child = map.PreviousButton; year < minYear && child.Click(); minYear = int.Parse(headerButton.Content.Split('-')[0]))
            {

            }
        }
        else if (year > maxYear)
        {
            for (var child = map.NextButton; year > maxYear && child.Click(); maxYear = int.Parse(headerButton.Content.Split('-')[1]))
            {

            }
        }
    }
}
