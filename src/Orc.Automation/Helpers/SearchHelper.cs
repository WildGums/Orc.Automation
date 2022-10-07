namespace Orc.Automation
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Media;

    public static class SearchHelper
    {
        public static UIElement? GetDirectlyOver(Window window, Point point, string typeName)
        {
            ArgumentNullException.ThrowIfNull(window);
            ArgumentNullException.ThrowIfNull(typeName);

            if (TryGetElementAtPosition(window, point, typeName, out var elementFromFilter, out var elementFromResult))
            {
                return elementFromFilter
                       ?? elementFromResult;
            }

            return null;
        }

        private static bool TryGetElementAtPosition(Window window, Point point, string typeName, [NotNullWhen(true)]out UIElement? elementFromFilter, [NotNullWhen(true)] out UIElement? elementFromResult)
        {
            ArgumentNullException.ThrowIfNull(window);
            ArgumentNullException.ThrowIfNull(typeName);

            elementFromFilter = null;
            elementFromResult = null;

            var pointHitTestParameters = new PointHitTestParameters(point);

            UIElement? elementFromFilterLocal = null;
            UIElement? elementFromResultLocal = null;
            VisualTreeHelper.HitTest(window, o => FilterCallback(o, typeName, ref elementFromFilterLocal), r => ResultCallback(r, ref elementFromResultLocal), pointHitTestParameters);

            elementFromFilter = elementFromFilterLocal;
            elementFromResult = elementFromResultLocal;

            return elementFromFilter is not null
                   || elementFromResult is not null;
        }

        private static HitTestFilterBehavior FilterCallback(DependencyObject target, string typeName, [NotNullWhen(true)] ref UIElement? element)
        {
            ArgumentNullException.ThrowIfNull(target);
            ArgumentNullException.ThrowIfNull(typeName);

            var filterResult = target switch
            {
                UIElement { IsVisible: false } => HitTestFilterBehavior.ContinueSkipSelfAndChildren,
                _ => HitTestFilterBehavior.Continue
            };

            if (target?.GetType().FullName?.EndsWith(typeName) == true)
            {
                element = target as UIElement;

                return HitTestFilterBehavior.Stop;
            }

            if (filterResult == HitTestFilterBehavior.Continue)
            {
                if (target is UIElement uiElement)
                {
                    element = uiElement;
                }
            }

            return filterResult;
        }

        private static HitTestResultBehavior ResultCallback(HitTestResult result, ref UIElement? directlyOverElement)
        {
            ArgumentNullException.ThrowIfNull(result);

            if (result.VisualHit is not UIElement uiElement)
            {
                return HitTestResultBehavior.Continue;
            }

            directlyOverElement = uiElement;
            return HitTestResultBehavior.Stop;
        }
    }
}
