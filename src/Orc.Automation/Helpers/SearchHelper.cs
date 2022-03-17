namespace Orc.Automation
{
    using System.Windows;
    using System.Windows.Media;

    public static class SearchHelper
    {
        public static UIElement GetDirectlyOver(Window window, Point point, string typeName)
        {
            if (TryGetElementAtPosition(window, point, typeName, out var elementFromFilter, out var elementFromResult))
            {
                return elementFromFilter
                       ?? elementFromResult;
            }

            return null;
        }

        private static bool TryGetElementAtPosition(Window window, Point point, string typeName, out UIElement elementFromFilter, out UIElement elementFromResult)
        {
            elementFromFilter = null;
            elementFromResult = null;

            var pointHitTestParameters = new PointHitTestParameters(point);

            UIElement elementFromFilterLocal = null;
            UIElement elementFromResultLocal = null;
            VisualTreeHelper.HitTest(window, o => FilterCallback(o, typeName, ref elementFromFilterLocal), r => ResultCallback(r, ref elementFromResultLocal), pointHitTestParameters);

            elementFromFilter = elementFromFilterLocal;
            elementFromResult = elementFromResultLocal;

            return elementFromFilter is not null
                   || elementFromResult is not null;
        }

        private static HitTestFilterBehavior FilterCallback(DependencyObject target, string typeName, ref UIElement element)
        {
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

        private static HitTestResultBehavior ResultCallback(HitTestResult result, ref UIElement directlyOverElement)
        {
            if (result?.VisualHit is not UIElement uiElement)
            {
                return HitTestResultBehavior.Continue;
            }

            directlyOverElement = uiElement;
            return HitTestResultBehavior.Stop;
        }
    }
}
