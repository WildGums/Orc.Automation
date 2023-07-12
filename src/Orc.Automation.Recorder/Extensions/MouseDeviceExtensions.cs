#nullable enable
namespace Orc.Automation;

using System.Reflection;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows;
using Win32;
using System;
using Orc.Automation.Helpers;

public static class MouseDeviceExtensions
{
    private static readonly PropertyInfo? RawDirectlyOverPropertyInfo
        = typeof(MouseDevice).GetProperty("RawDirectlyOver", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

    public static UIElement? GetDirectlyOver(this MouseDevice mouseDevice)
    {
        if (TryGetElementAtMousePosition(mouseDevice.Dispatcher, out var elementFromFilter, out var elementFromResult))
        {
            return elementFromFilter
                   ?? elementFromResult;
        }

        var elementMouseRawDirectlyOver = RawDirectlyOverPropertyInfo?.GetValue(mouseDevice, null) as UIElement;
        var elementMouseDeviceDirectlyOver = mouseDevice.DirectlyOver as UIElement;

        return elementMouseRawDirectlyOver
               ?? elementMouseDeviceDirectlyOver;
    }

    public static bool TryGetElementAtPosition(Window window, Point mousePosition, out UIElement? elementFromFilter, out UIElement? elementFromResult)
    {
        elementFromFilter = null;
        elementFromResult = null;

        var pointHitTestParameters = new PointHitTestParameters(mousePosition);

        UIElement? elementFromFilterLocal = null;
        UIElement? elementFromResultLocal = null;
        VisualTreeHelper.HitTest(window, o => FilterCallback(o, ref elementFromFilterLocal), r => ResultCallback(r, ref elementFromResultLocal), pointHitTestParameters);

        elementFromFilter = elementFromFilterLocal;
        elementFromResult = elementFromResultLocal;

        return elementFromFilter is not null
               || elementFromResult is not null;
    }

    private static bool TryGetElementAtMousePosition(Dispatcher dispatcher, out UIElement? elementFromFilter, out UIElement? elementFromResult)
    {
        elementFromFilter = null;
        elementFromResult = null;

        var windowHandleUnderMouse = WindowHelper.GetWindowUnderMouse();
        var windowUnderMouse = WindowHelper.GetVisibleWindow(windowHandleUnderMouse, dispatcher);

        if (windowUnderMouse is null)
        {
            return false;
        }

        var mousePosition = MouseHelper.TryGetRelativeMousePosition(windowHandleUnderMouse, out var nativeMousePosition)
            ? DpiHelper.DevicePixelsToLogical(nativeMousePosition, windowHandleUnderMouse)
            : Mouse.GetPosition(windowUnderMouse);

        var pointHitTestParameters = new PointHitTestParameters(mousePosition);

        UIElement? elementFromFilterLocal = null;
        UIElement? elementFromResultLocal = null;
        VisualTreeHelper.HitTest(windowUnderMouse, o => FilterCallback(o, ref elementFromFilterLocal), r => ResultCallback(r, ref elementFromResultLocal), pointHitTestParameters);

        elementFromFilter = elementFromFilterLocal;
        elementFromResult = elementFromResultLocal;

        return elementFromFilter is not null
               || elementFromResult is not null;
    }

    private static HitTestFilterBehavior FilterCallback(DependencyObject target, ref UIElement? element)
    {
        var filterResult = target switch
        {
            UIElement { IsVisible: false } => HitTestFilterBehavior.ContinueSkipSelfAndChildren,
          //  UIElement uiElement when uiElement.IsPartOfSnoopVisualTree() => HitTestFilterBehavior.ContinueSkipSelfAndChildren,
            _ => HitTestFilterBehavior.Continue
        };

        if (filterResult == HitTestFilterBehavior.Continue)
        {
            if (target is UIElement uiElement)
            {
                element = uiElement;
            }
        }

        return filterResult;
    }

    private static HitTestResultBehavior ResultCallback(HitTestResult? result, ref UIElement? directlyOverElement)
    {
        if (result?.VisualHit is not UIElement uiElement)
        {
            return HitTestResultBehavior.Continue;
        }

        directlyOverElement = uiElement;
        return HitTestResultBehavior.Stop;
    }
}
