namespace Orc.Automation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;
using System.Windows.Automation.Text;

public static partial class AutomationElementExtensions
{
    #region Get Value
    public static T GetValue<T>(this AutomationElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        if (typeof(T) == typeof(double))
        {
            return (T)(object)element.RunPatternFunc<RangeValuePattern, double>(x => x.Current.Value);
        }

        if (typeof(T) == typeof(string))
        {
            return (T)(object)element.RunPatternFunc<ValuePattern, string>(x => x.Current.Value);
        }

        throw new AutomationException("Can't get value");
    }

    public static bool TryGetValue(this AutomationElement element, out string value)
    {
        ArgumentNullException.ThrowIfNull(element);

        var localValue = string.Empty;
        value = string.Empty;

        var result = element.TryRunPatternFunc<ValuePattern>(x => localValue = x.Current.Value);
        if (result)
        {
            value = localValue;

            return true;
        }

        return false;
    }

    public static bool TryGetValue(this AutomationElement element, out double value)
    {
        ArgumentNullException.ThrowIfNull(element);

        var localValue = 0d;
        value = 0d;

        var result = element.TryRunPatternFunc<RangeValuePattern>(x => localValue = x.Current.Value);
        if (result)
        {
            value = localValue;

            return true;
        }

        return false;
    }
    #endregion

    #region Set Value
    public static void SetValue(this AutomationElement element, double value)
    {
        ArgumentNullException.ThrowIfNull(element);

        element.RunPatternFunc<RangeValuePattern>(x => x.SetValue(value));
    }

    public static void SetValue(this AutomationElement element, string value)
    {
        ArgumentNullException.ThrowIfNull(element);

        element.RunPatternFunc<ValuePattern>(x => x.SetValue(value));
    }

    public static bool TrySetValue(this AutomationElement element, double value)
    {
        ArgumentNullException.ThrowIfNull(element);

        return element.TryRunPatternFunc<RangeValuePattern>(x => x.SetValue(value));
    }

    public static bool TrySetValue(this AutomationElement element, string value)
    {
        ArgumentNullException.ThrowIfNull(element);

        return element.TryRunPatternFunc<ValuePattern>(x => x.SetValue(value));
    }
    #endregion

    #region Text
    public static IReadOnlyList<string> GetSelectedTextRanges(this AutomationElement element)
    {
        var textRanges = element.RunPatternFunc<TextPattern, TextPatternRange[]>(x => x.GetSelection());

        var result = textRanges?.Select(x => x.GetText(-1)).ToArray() ?? Array.Empty<string>();
        return result;
    }
    #endregion

    #region Range
    public static double GetRangeMinimum(this AutomationElement element)
    {
        return element.RunPatternFunc<RangeValuePattern, double>(x => x.Current.Minimum);
    }

    public static double GetRangeMaximum(this AutomationElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        return element.RunPatternFunc<RangeValuePattern, double>(x => x.Current.Maximum);
    }

    public static double GetRangeSmallChange(this AutomationElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        return element.RunPatternFunc<RangeValuePattern, double>(x => x.Current.SmallChange);
    }

    public static double GetRangeLargeChange(this AutomationElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        return element.RunPatternFunc<RangeValuePattern, double>(x => x.Current.LargeChange);
    }

    public static bool TryGetRangeSmallChange(this AutomationElement element, out double smallChange)
    {
        ArgumentNullException.ThrowIfNull(element);

        return element.TryRunPatternFunc<RangeValuePattern, double>(x => x.Current.SmallChange, out smallChange);
    }

    public static bool TryGetRangeLargeChange(this AutomationElement element, out double largeChange)
    {
        ArgumentNullException.ThrowIfNull(element);

        return element.TryRunPatternFunc<RangeValuePattern, double>(x => x.Current.LargeChange, out largeChange);
    }

    public static bool TryGetRangeMinimum(this AutomationElement element, out double minimum)
    {
        ArgumentNullException.ThrowIfNull(element);

        return element.TryRunPatternFunc<RangeValuePattern, double>(x => x.Current.Minimum, out minimum);
    }

    public static bool TryGetRangeMaximum(this AutomationElement element, out double maximum)
    {
        ArgumentNullException.ThrowIfNull(element);

        return element.TryRunPatternFunc<RangeValuePattern, double>(x => x.Current.Maximum, out maximum);
    }
    #endregion

    #region Select
    public static void Select(this AutomationElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        element.RunPatternFunc<SelectionItemPattern>(x => x.Select());
    }

    public static bool GetIsSelected(this AutomationElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        return element.RunPatternFunc<SelectionItemPattern, bool>(x => x.Current.IsSelected);
    }

    public static bool TrySetSelection(this AutomationElement element, bool isSelected)
    {
        if (!TryGetIsSelected(element, out var isCurrentlySelected))
        {
            return false;
        }

        if (Equals(isSelected, isCurrentlySelected))
        {
            return true;
        }

        if (isSelected)
        {
            var container = element.GetSelectionContainer();
            var canSelectMultiply = container.CanSelectMultiple();
            if (canSelectMultiply)
            {
                return TryAddToSelection(element);
            }

            return TrySelect(element);
        }

        return TryDeselect(element);
    }

    public static AutomationElement GetSelectionContainer(this AutomationElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        return element.RunPatternFunc<SelectionItemPattern, AutomationElement>(x => x.Current.SelectionContainer);
    }

    public static bool TryAddToSelection(this AutomationElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        return element.TryRunPatternFunc<SelectionItemPattern>(x => x.AddToSelection());
    }

    public static bool TrySelect(this AutomationElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        return element.TryRunPatternFunc<SelectionItemPattern>(x => x.Select());
    }

    public static bool TryDeselect(this AutomationElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        return element.TryRunPatternFunc<SelectionItemPattern>(x => x.RemoveFromSelection());
    }

    public static bool TryGetIsSelected(this AutomationElement element, out bool isSelected)
    {
        ArgumentNullException.ThrowIfNull(element);

        isSelected = false;
        var localIsSelected = false;
        if (element.TryRunPatternFunc<SelectionItemPattern>(x => localIsSelected = x.Current.IsSelected))
        {
            isSelected = localIsSelected;

            return true;
        }

        return false;
    }
    #endregion

    #region Select Item
    public static AutomationElement[] GetSelection(this AutomationElement container)
    {
        ArgumentNullException.ThrowIfNull(container);

        return container.RunPatternFunc<SelectionPattern, AutomationElement[]>(x => x.Current.GetSelection());
    }

    public static bool CanSelectMultiple(this AutomationElement container)
    {
        ArgumentNullException.ThrowIfNull(container);

        return container.RunPatternFunc<SelectionPattern, bool>(x => x.Current.CanSelectMultiple);
    }

    public static bool TrySelectItem(this AutomationElement containerElement, int index, out AutomationElement selectItem)
    {
        ArgumentNullException.ThrowIfNull(containerElement);

        selectItem = GetChild(containerElement, index);
        return selectItem?.TrySelect() == true;
    }
    #endregion

    #region Invoke
    public static void Invoke(this AutomationElement element)
    {
        element.RunPatternFunc<InvokePattern>(x => x.Invoke());
    }

    public static bool TryInvoke(this AutomationElement element)
    {
        return element.TryRunPatternFunc<InvokePattern>(x => x.Invoke());
    }
    #endregion

    #region Table
    public static AutomationElement[] GetColumnHeaders(this AutomationElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        return element.RunPatternFunc<TablePattern, AutomationElement[]>(x => x.Current.GetColumnHeaders());
    }

    public static AutomationElement[] GetRowHeaders(this AutomationElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        return element.RunPatternFunc<TablePattern, AutomationElement[]>(x => x.Current.GetRowHeaders());
    }

    public static int GetRowCount(this AutomationElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        return element.RunPatternFunc<TablePattern, int>(x => x.Current.RowCount);
    }

    public static int GetColumnCount(this AutomationElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        return element.RunPatternFunc<TablePattern, int>(x => x.Current.ColumnCount);
    }

    public static AutomationElement GetTableItem(this AutomationElement element, int row, int column)
    {
        ArgumentNullException.ThrowIfNull(element);

        return element.RunPatternFunc<TablePattern, AutomationElement>(x => x.GetItem(row, column));
    }
    #endregion
        
    #region Transform
    public static void Resize(this AutomationElement element, double newWidth, double newHeight)
    {
        ArgumentNullException.ThrowIfNull(element);

        element.TryRunPatternFunc<TransformPattern>(x =>
        {
            if (x.Current.CanResize)
            {
                x.Resize(newWidth, newHeight);
            }
        });
    }

    public static void Move(this AutomationElement element, double newX, double newY)
    {
        ArgumentNullException.ThrowIfNull(element);

        element.TryRunPatternFunc<TransformPattern>(x =>
        {
            if (x.Current.CanMove)
            {
                x.Move(newX, newY);
            }
        });
    }

    public static void Rotate(this AutomationElement element, double degrees)
    {
        ArgumentNullException.ThrowIfNull(element);

        element.TryRunPatternFunc<TransformPattern>(x =>
        {
            if (x.Current.CanRotate)
            {
                x.Rotate(degrees);
            }
        });
    }
    #endregion

    #region Toggle
    /// <summary>
    /// Toggle element if TogglePattern is available
    /// </summary>
    /// <param name="element">input element</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">if element is null</exception>
    /// <exception cref="AutomationException">if Toggle pattern is not supported</exception>
    public static bool? Toggle(this AutomationElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        if (TryToggle(element, out var state))
        {
            return state;
        }

        throw new AutomationException("Can't toggle, pattern not available");
    }

    public static bool TrySetToggleState(this AutomationElement element, bool? newState)
    {
        if (!TryGetToggleState(element, out var toggleState))
        {
            return false;
        }

        if (toggleState == newState)
        {
            return true;
        }

        if (!TryToggle(element, out toggleState))
        {
            return false;
        }
            
        if (toggleState == newState)
        {
            return true;
        }

        if (!TryToggle(element, out toggleState))
        {
            return false;
        }

        if (toggleState == newState)
        {
            return true;
        }

        //After 2 toggles result doesn't match requested value
        return false;
    }

    public static bool? GetToggleState(this AutomationElement element)
    {
        if (TryGetToggleState(element, out var toggleState))
        {
            return toggleState;
        }

        throw new AutomationException("Can't get toggle state");
    }

    public static bool TryToggle(this AutomationElement element)
    {
        return element.TryRunPatternFunc<TogglePattern>(x => x.Toggle());
    }

    public static bool TryToggle(this AutomationElement element, out bool? toggleState)
    {
        toggleState = null;

        return TryToggle(element) && TryGetToggleState(element, out toggleState);
    }

    public static bool TryGetToggleState(this AutomationElement element, out bool? toggleState)
    {
        var state = ToggleState.Off;

        var result = element.TryRunPatternFunc<TogglePattern>(x => state = x.Current.ToggleState);

        toggleState = state switch
        {
            ToggleState.Off => false,
            ToggleState.On => true,
            ToggleState.Indeterminate => null,
            _ => throw new ArgumentOutOfRangeException()
        };

        return result;
    }
    #endregion

    #region Expand/Collapse
    /// <summary>
    /// 
    /// </summary>
    /// <param name="element"></param>
    public static void Expand(this AutomationElement element)
    {
        element.RunPatternFunc<ExpandCollapsePattern>(x => x.Expand());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="element"></param>
    public static void Collapse(this AutomationElement element)
    {
        element.RunPatternFunc<ExpandCollapsePattern>(x => x.Collapse());
    }

    public static bool GetIsExpanded(this AutomationElement element)
    {
        return element.RunPatternFunc<ExpandCollapsePattern, ExpandCollapseState>(x => x.Current.ExpandCollapseState) == ExpandCollapseState.Expanded;
    }

    public static void SetIsExpanded(this AutomationElement element, bool isExpanded)
    {
        var isExpandedInCurrentState = GetIsExpanded(element);
        if (Equals(isExpandedInCurrentState, isExpanded))
        {
            return;
        }

        if (isExpanded)
        {
            TryExpand(element);
        }
        else
        {
            TryCollapse(element);
        }
    }

    public static bool TryExpand(this AutomationElement element)
    {
        return element.TryRunPatternFunc<ExpandCollapsePattern>(x => x.Expand());
    }

    public static bool TryCollapse(this AutomationElement element)
    {
        return element.TryRunPatternFunc<ExpandCollapsePattern>(x => x.Collapse());
    }
    #endregion

    #region MultipleViewPattern
    public static void SelectView(this AutomationElement element, int viewIndex)
    {
        ArgumentNullException.ThrowIfNull(element);

        element.TryRunPatternFunc<MultipleViewPattern>(x =>
        {
            var supportedViews = x.Current.GetSupportedViews();
            if (supportedViews.Length >= viewIndex)
            {
                x.SetCurrentView(supportedViews[viewIndex]);
            }
        });
    }

    //TODO:Vladimir:maybe use IDisposable to return in previous view...but for now lets leave that way
    public static TResult InvokeInView<TResult>(this AutomationElement element, int viewIndex, Func<TResult>? func)
    {
        ArgumentNullException.ThrowIfNull(element);

        if (func is null)
        {
            return default!;
        }

        element.SelectView(viewIndex);

        return func.Invoke();
    }
    #endregion

    #region Window
    public static void CloseWindow(this AutomationElement element)
    {
        element.RunPatternFunc<WindowPattern>(x => x.Close());
    }

    public static bool TryCloseWindow(this AutomationElement element)
    {
        return element.TryRunPatternFunc<WindowPattern>(x => x.Close());
    }
    #endregion

    #region Grid
    public static AutomationElement GetGridItem(this AutomationElement element, int row, int column)
    {
        ArgumentNullException.ThrowIfNull(element);

        return element.RunPatternFunc<GridPattern, AutomationElement>(x => x.GetItem(row, column));
    }
    #endregion

    #region Scroll
    public static void ScrollIntoView(this AutomationElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        element.RunPatternFunc<ScrollItemPattern>(x => x.ScrollIntoView());
    }
    #endregion

    /// <summary>
    /// Try to Invoke, Toggle, Select...if this patterns not implemented, depends on useMouse parameter use Mouse Input
    /// </summary>
    /// <param name="element"></param>
    /// <param name="useMouse"></param>
    /// <returns></returns>
    public static bool TryClick(this AutomationElement element, bool useMouse = true)
    {
        ArgumentNullException.ThrowIfNull(element);

        try
        {
            if (!element.TryInvoke() && !element.TryToggle() && !element.TrySelect() && useMouse)
            {
                MouseClick(element);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }

        return true;
    }
        
    public static TResult RunPatternFunc<TPattern, TResult>(this AutomationElement element, Func<TPattern, TResult> func)
        where TPattern : BasePattern
    {
        ArgumentNullException.ThrowIfNull(element);

        return TryRunPatternFunc(element, func, out var funcResult)
            ? funcResult
            : throw new AutomationException($"Can't run pattern {typeof(TPattern).Name}");
    }

    public static void RunPatternFunc<TPattern>(this AutomationElement element, Action<TPattern> action)
        where TPattern : BasePattern
    {
        ArgumentNullException.ThrowIfNull(element);

        var result = TryRunPatternFunc(element, action);
        if (!result)
        {
            throw new AutomationException($"Can't run pattern {typeof(TPattern).Name}");
        }
    }

    public static bool TryRunPatternFunc<TPattern, TResult>(this AutomationElement element, Func<TPattern, TResult> func, out TResult functionResult)
        where TPattern : BasePattern
    {
        ArgumentNullException.ThrowIfNull(element);

        functionResult = default;
        TResult localFuncResult = default;
        if (TryRunPatternFunc(element, (TPattern pattern) => localFuncResult = func(pattern)))
        {
            functionResult = localFuncResult;
            return true;
        }

        return false;
    }

    public static bool TryRunPatternFunc<TPattern>(this AutomationElement element, Action<TPattern> action)
        where TPattern : BasePattern
    {
        ArgumentNullException.ThrowIfNull(element);

        var automationPattern = TryGetPattern<TPattern>(element);
        if (automationPattern is null)
        {
            return false;
        }

        try
        {
            action?.Invoke(automationPattern);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            return false;
        }

        return true;
    }
        
    public static TPattern? TryGetPattern<TPattern>(this AutomationElement element)
        where TPattern : BasePattern
    {
        var patternField = typeof(TPattern).GetField("Pattern");
        if (patternField?.GetValue(null) is not AutomationPattern pattern)
        {
            return null;
        }

        var supportedPatterns = element.GetSupportedPatterns();
        var automationPattern = supportedPatterns?.FirstOrDefault(x => x.ProgrammaticName == pattern.ProgrammaticName);
        if (automationPattern is null)
        {
            return null;
        }

        var currentPattern = element.GetCurrentPattern(automationPattern) as TPattern;

        return currentPattern;
    }
}
