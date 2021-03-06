namespace Orc.Automation
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Automation;
    using Catel;
    using Catel.IoC;
    using Catel.Windows;
    using Microsoft.Xaml.Behaviors;

    public static class FrameworkElementExtensions
    {
        public static DependencyObject FindVisualDescendantWithAutomationId(this System.Windows.FrameworkElement frameworkElement, string targetId)
        {
            Argument.IsNotNull(() => frameworkElement);
            
            var result = frameworkElement.FindVisualDescendant(x => Equals((x as System.Windows.FrameworkElement)?.GetValue(AutomationProperties.AutomationIdProperty), targetId));

            return result ?? frameworkElement.FindVisualDescendantByName(targetId);
        }

        public static TBehavior AttachBehavior<TBehavior>(this System.Windows.FrameworkElement frameworkElement)
            where TBehavior : Behavior
        {
            Argument.IsNotNull(() => frameworkElement);

            var behaviors = Interaction.GetBehaviors(frameworkElement);

            var existingBehaviorOfType = behaviors.OfType<TBehavior>().FirstOrDefault();
            if (existingBehaviorOfType is not null)
            {
                return existingBehaviorOfType;
            }

            var behavior = frameworkElement.GetTypeFactory().CreateInstanceWithParametersAndAutoCompletion<TBehavior>();
            behaviors.Add(behavior);

            return behavior;
        }
    }
}
