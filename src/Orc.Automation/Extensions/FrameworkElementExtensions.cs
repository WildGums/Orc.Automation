namespace Orc.Automation;

using System;
using System.Linq;
using System.Windows;
using System.Windows.Automation;
using Catel.IoC;
using Catel.Windows;
using Microsoft.Xaml.Behaviors;

public static class FrameworkElementExtensions
{
    public static DependencyObject? FindVisualDescendantWithAutomationId(this FrameworkElement frameworkElement, string targetId)
    {
        ArgumentNullException.ThrowIfNull(frameworkElement);
            
        var result = frameworkElement.FindVisualDescendant(x => Equals((x as FrameworkElement)?.GetValue(AutomationProperties.AutomationIdProperty), targetId));

        return result ?? frameworkElement.FindVisualDescendantByName(targetId);
    }

    public static TBehavior AttachBehavior<TBehavior>(this FrameworkElement frameworkElement)
        where TBehavior : Behavior
    {
        ArgumentNullException.ThrowIfNull(frameworkElement);

        var behaviors = Interaction.GetBehaviors(frameworkElement);

        var existingBehaviorOfType = behaviors.OfType<TBehavior>().FirstOrDefault();
        if (existingBehaviorOfType is not null)
        {
            return existingBehaviorOfType;
        }

#pragma warning disable IDISP004 // Don't ignore created IDisposable
        var behavior = frameworkElement.GetTypeFactory().CreateRequiredInstanceWithParametersAndAutoCompletion<TBehavior>();
#pragma warning restore IDISP004 // Don't ignore created IDisposable
        behaviors.Add(behavior);

        return behavior;
    }
}