namespace Orc.Automation;

using System.Windows;
using Automation;

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
