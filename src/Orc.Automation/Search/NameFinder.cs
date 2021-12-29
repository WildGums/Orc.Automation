namespace Orc.Automation
{
    using System.Windows;
    using Automation;
    using Catel.Windows;

    public class FinderSearchContext
    {
        public string Name { get; set; }
        public string ClassName { get; set; }
    }

    public class SearchContextFinder : ConditionalPartFinderBase
    {
        public static SearchContextFinder Create(SearchContext searchContext) => new()
        {
            SearchContext = new FinderSearchContext
            {
                Name = searchContext.Name,
                ClassName = searchContext.ClassName
            }
        };

        public FinderSearchContext SearchContext { get; set; }

        protected override bool IsMatch(object descendant)
        {
            var isMatch = true;

            var name = SearchContext.Name;
            if (!string.IsNullOrWhiteSpace(name))
            {
                isMatch &= Equals(((DependencyObject)descendant).GetValue(FrameworkElement.NameProperty), name);
            }

            var className = SearchContext.ClassName;
            if (isMatch && !string.IsNullOrWhiteSpace(className))
            {
                isMatch &= Equals(descendant.GetType().FullName, className);
            }

            return isMatch;
        }
    }

    public class ClassFinder : ConditionalPartFinderBase
    {
        public static ClassFinder Create(string className) => new()
        {
            ClassName = className
        };

        public string ClassName { get; set; }

        protected override bool IsMatch(object descendant)
        {
            return Equals(descendant.GetType().FullName, ClassName);
        }
    }

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

    public abstract class ConditionalPartFinderBase : IPartFinder
    {
        protected abstract bool IsMatch(object descendant);

        public FrameworkElement Find(FrameworkElement parent)
        {
            return parent.FindVisualDescendant(IsMatch) as FrameworkElement;
        }
    }
}
