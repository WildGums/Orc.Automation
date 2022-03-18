namespace Orc.Automation
{
    using System.Windows;

    public class NameFinder : ConditionalPartFinderBase
    {
        public static NameFinder Create(string name) => new()
        {
            Name = name
        };

        public string Name { get; set; }

        protected override bool IsMatch(object descendant)
        {
            return Equals(((DependencyObject)descendant).GetValue(FrameworkElement.NameProperty), Name);
        }
    }
}
