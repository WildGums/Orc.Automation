namespace Orc.Automation
{
    using System.Linq;
    using Catel;
    using Catel.IoC;
    using Controls;
    using Microsoft.Xaml.Behaviors;

    public static class FrameworkElementExtensions
    {
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

        public static bool IsVisible(this FrameworkElement element)
        {
            //Automation can't find element if it's not visible, so no checks for null
            if (element is null)
            {
                return false;
            }

            return element.Element.IsVisible();
        }
    }
}
