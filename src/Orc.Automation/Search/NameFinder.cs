namespace Orc.Automation
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
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
            return FindVisualDescendant(parent, IsMatch) as FrameworkElement;
        }


        private static DependencyObject FindVisualDescendant(DependencyObject startElement, Predicate<object> condition)
        {
            if (startElement is not null)
            {
                if (condition(startElement))
                {
                    return startElement;
                }

                if (startElement is UserControl startElementAsUserControl)
                {
                    return FindVisualDescendant(startElementAsUserControl.Content as DependencyObject, condition);
                }

                if (startElement is ContentControl startElementAsContentControl)
                {
                    var result = FindVisualDescendant(startElementAsContentControl.Content as DependencyObject, condition);
                    if (result is not null)
                    {
                        return result;
                    }
                }

                if (startElement is Border startElementAsBorder)
                {
                    return FindVisualDescendant(startElementAsBorder.Child, condition);
                }

#if NET || NETCORE
                if (startElement is Decorator startElementAsDecorator)
                {
                    return FindVisualDescendant(startElementAsDecorator.Child, condition);
                }
#endif

                // If the element has children, loop the children
                var children = new List<DependencyObject>();

                for (var i = 0; i < VisualTreeHelper.GetChildrenCount(startElement); i++)
                {
                    children.Add(VisualTreeHelper.GetChild(startElement, i));
                }

                // First, loop children itself
                foreach (var child in children)
                {
                    if (condition(child))
                    {
                        return child;
                    }
                }

                // Direct child is not what we are looking for, continue
                foreach (var child in children)
                {
                    var obj = FindVisualDescendant(child, condition);
                    if (obj is not null)
                    {
                        return obj;
                    }
                }
            }

            return null;
        }
    }
}
