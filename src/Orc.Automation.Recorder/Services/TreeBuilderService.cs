namespace Orc.Automation.Services;

using System.Linq;
using System.Windows;
using System.Windows.Media;
using AutomationMethods;
using Catel.Reflection;

public class TreeBuilderService
{
    public void Build(AutomationInformer informer)
    {
        var allActive = informer.Element.GetDescendants()
            .Where(AutomationHelper.IsActiveModelControl)
            .ToList();

        var dataGrid = informer.Find(className: "Gum.Controls.DataGrid" + NameConventions.ActiveModelControlClassNameSuffix);
        var accessor = new AutomationElementAccessor(dataGrid);

        accessor.AutomationEvent += OnEvent;

        var dataGridClickablePoint = dataGrid.Current.BoundingRectangle.GetClickablePoint(); 
        var windowRect = informer.AutomationProperties.BoundingRectangle;
        var point = new Point(dataGridClickablePoint.X - windowRect.Left, dataGridClickablePoint.Y - windowRect.Top);

        informer.Execute<LoadAssemblyMethodRun>(GetType().Assembly.Location);
        var result = informer.Execute<TestMethodRun>(point, dataGrid.Current.ClassName);
    }

    private void OnEvent(object sender, AutomationEventArgs e)
    {
        
    }
}

public class TestMethodRun : NamedAutomationMethodRun
{
    public override bool TryInvoke(FrameworkElement owner, AutomationMethod method, out AutomationValue result)
    {
        result = AutomationValue.FromValue(true);

        if (owner is not Controls.AutomationInformer informer)
        {
            return false;
        }

        var parameters = method.Parameters;

        var point = (Point)parameters[0].ExtractValue();
        var className = (string)parameters[1].ExtractValue();
        className = className.Replace(NameConventions.ActiveModelControlClassNameSuffix, string.Empty);

        Wait.UntilInputProcessed(2000);
        
        var pointHitTestParameters = new PointHitTestParameters(point);

        UIElement elementFromFilterLocal = null;
        UIElement elementFromResultLocal = null;
        VisualTreeHelper.HitTest(Application.Current.MainWindow, o => FilterCallback(o, className, ref elementFromFilterLocal),
            r => ResultCallback(r, ref elementFromResultLocal), pointHitTestParameters);

        return true;
    }

    private static HitTestFilterBehavior FilterCallback(DependencyObject target, string className, ref UIElement element)
    {
        var filterResult = target switch
        {
            UIElement { IsVisible: false } => HitTestFilterBehavior.ContinueSkipSelfAndChildren,
            _ => HitTestFilterBehavior.Continue
        };

        if (target.GetType().GetSafeFullName() == className)
        {
            element = target as UIElement;
            filterResult = HitTestFilterBehavior.Stop;
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
