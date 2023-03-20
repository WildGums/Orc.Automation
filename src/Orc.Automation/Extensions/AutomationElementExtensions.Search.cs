namespace Orc.Automation;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Automation;
using Automation;
using Controls;

public static partial class AutomationElementExtensions
{
    public static Window? GetHostWindow(this AutomationElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        return element.Find<Window>(controlType: ControlType.Window, scope: TreeScope.Ancestors);
    }

    public static TAutomationElementOrElementCollection FindOneOrMany<TAutomationElementOrElementCollection>(this AutomationElement element, SearchContext searchContext)
    {
        ArgumentNullException.ThrowIfNull(element);

        object? searchResult;
        var type = typeof(TAutomationElementOrElementCollection);
        if (typeof(IEnumerable).IsAssignableFrom(type))
        {
            searchResult = element.FindAll(id: searchContext.Id, name: searchContext.Name, className: searchContext.ClassName, controlType: searchContext.ControlType);
        }
        else
        {
            searchResult = //!searchInfo.IsTransient && _targetControl is not null
                //  ? _targetControl.GetPart(id: searchInfo.AutomationId, name: searchInfo.Name, className: searchInfo.ClassName, controlType: searchInfo.ControlType)
                // :
                element.Find(id: searchContext.Id, name: searchContext.Name, className: searchContext.ClassName, controlType: searchContext.ControlType);
        }

        searchResult = (TAutomationElementOrElementCollection)AutomationHelper.WrapAutomationObject(type, searchResult);

        return (TAutomationElementOrElementCollection)searchResult;
    }


    public static TElement? Find<TElement>(this AutomationElement element, string? id = null, string? name = null, string? className = null, bool isRaw = false, ControlType? controlType = null, TreeScope scope = TreeScope.Descendants, int numberOfWaits = SearchParameters.NumberOfWaits)
        where TElement : AutomationControl
    {
        return Find<TElement>(element, new SearchContext(id, name, className, controlType, isRaw), scope, numberOfWaits);
    }

    public static AutomationElement? Find(this AutomationElement element, string? id = null, string? name = null, string? className = null, bool isRaw = false, ControlType? controlType = null, TreeScope scope = TreeScope.Descendants, int numberOfWaits = SearchParameters.NumberOfWaits)
    {
        return Find(element, new SearchContext(id, name, className, controlType, isRaw), scope, numberOfWaits);
    }

    public static TElement? Find<TElement>(this AutomationElement element, SearchContext searchContext, TreeScope scope = TreeScope.Descendants, int numberOfWaits = SearchParameters.NumberOfWaits)
        where TElement : AutomationControl
    {
        return (TElement?)Find(element, searchContext, typeof(TElement), scope, numberOfWaits);
    }

    public static object? Find(this AutomationElement element, SearchContext searchContext, Type wrapperType, TreeScope scope = TreeScope.Descendants, int numberOfWaits = SearchParameters.NumberOfWaits)
    {
        var className = AutomationHelper.GetControlClassName(wrapperType);
        if (!string.IsNullOrWhiteSpace(className) && string.IsNullOrWhiteSpace(searchContext.ClassName))
        {
            searchContext.ClassName = className;
        }

        var controlType = AutomationHelper.GetControlType(wrapperType);
        if (controlType is not null && searchContext.ControlType is null)
        {
            searchContext.ControlType = controlType;
        }

        var foundElement = Find(element, searchContext, scope, numberOfWaits);
        if (foundElement is null)
        {
            return null;
        }

        return AutomationHelper.WrapAutomationObject(wrapperType, foundElement);
    }

    public static AutomationElement? Find(this AutomationElement element, SearchContext searchContext, TreeScope scope = TreeScope.Descendants, int numberOfWaits = SearchParameters.NumberOfWaits)
    {
        var id = searchContext.Id;
        var name = searchContext.Name;
        var controlType = searchContext.ControlType;
        var className = searchContext.ClassName;
        var isRaw = searchContext.IsRaw;

        var conditions = new List<Condition>();
        if (!string.IsNullOrWhiteSpace(id))
        {
            conditions.Add(new PropertyCondition(AutomationElement.AutomationIdProperty, id));
        }

        if (!string.IsNullOrWhiteSpace(name))
        {
            conditions.Add(new PropertyCondition(AutomationElement.NameProperty, name));
        }

        if (!string.IsNullOrWhiteSpace(className))
        {
            conditions.Add(new PropertyCondition(AutomationElement.ClassNameProperty, className));
        }

        if (controlType is not null)
        {
            conditions.Add(new PropertyCondition(AutomationElement.ControlTypeProperty, controlType));
        }

        //if (isRaw)
        //{
        //    conditions.Add(System.Windows.Automation.Automation.RawViewCondition);
        //}

        if (!conditions.Any())
        {
            return null;
        }

        var resultCondition = conditions.Count == 1
            ? conditions[0]
            : new AndCondition(conditions.ToArray());

        return Find(element, resultCondition, scope, isRaw, numberOfWaits);
    }

    private static AutomationElement? Find(this AutomationElement element, Condition condition, TreeScope scope = TreeScope.Descendants, bool isRaw = false, int numberOfWaits = SearchParameters.NumberOfWaits)
    {
        ArgumentNullException.ThrowIfNull(element);

        var numWaits = 0;

        AutomationElement? foundElement;
        do
        {
            foundElement = TryFindElementByCondition(element, scope, condition, isRaw); 

            ++numWaits;
            Thread.Sleep(SearchParameters.WaitDelay);
        } 
        while (foundElement is null && numWaits < numberOfWaits);

        return foundElement;
    }

    private static AutomationElement? TryFindElementByCondition(this AutomationElement element, TreeScope scope, Condition condition, bool isRaw)
    {
        if (scope is TreeScope.Parent or TreeScope.Ancestors)
        {
            return element.GetParent(condition);
        }

        if (isRaw)
        {
            var andCondition = new AndCondition(condition, System.Windows.Automation.Automation.RawViewCondition);
            return new TreeWalker(andCondition).GetFirstChild(element);
        }

        return element.FindFirst(scope, condition);
    }

    public static IEnumerable<TElement> FindAll<TElement>(this AutomationElement element, string? id = null, string? name = null, string? className = null, bool isRaw = false, ControlType? controlType = null, TreeScope scope = TreeScope.Descendants, int numberOfWaits = SearchParameters.NumberOfWaits)
        where TElement : AutomationControl
    {
        return FindAll<TElement>(element, new SearchContext(id, name, className, controlType, isRaw), scope, numberOfWaits);
    }

    public static IEnumerable<AutomationElement> FindAll(this AutomationElement element, string? id = null, string? name = null, string? className = null, bool isRaw = false, ControlType? controlType = null, TreeScope scope = TreeScope.Descendants, int numberOfWaits = SearchParameters.NumberOfWaits)
    {
        return FindAll(element, new SearchContext(id, name, className, controlType, isRaw), scope, numberOfWaits);
    }

    public static IEnumerable<TElement> FindAll<TElement>(this AutomationElement element, SearchContext searchContext, TreeScope scope = TreeScope.Descendants, int numberOfWaits = SearchParameters.NumberOfWaits)
        where TElement : AutomationControl
    {
        return (IEnumerable<TElement>)FindAll(element, searchContext, typeof(TElement), scope, numberOfWaits);
    }

    public static object FindAll(this AutomationElement element, SearchContext searchContext, Type wrapperType, TreeScope scope = TreeScope.Descendants, int numberOfWaits = SearchParameters.NumberOfWaits)
    {
        var className = AutomationHelper.GetControlClassName(wrapperType);
        if (!string.IsNullOrWhiteSpace(className) && string.IsNullOrWhiteSpace(searchContext.ClassName))
        {
            searchContext.ClassName = className;
        }

        var controlType = AutomationHelper.GetControlType(wrapperType);
        if (controlType is not null && searchContext.ControlType is null)
        {
            searchContext.ControlType = controlType;
        }

        var foundElement = FindAll(element, searchContext, scope, numberOfWaits);
        if (foundElement is null)
        {
            return Array.Empty<AutomationElement>();
        }

        var list = typeof(List<>).MakeGenericType(wrapperType);

        return AutomationHelper.WrapAutomationObject(list, foundElement);
    }

    public static IEnumerable<AutomationElement> FindAll(this AutomationElement element, SearchContext searchContext, TreeScope scope = TreeScope.Descendants, int numberOfWaits = SearchParameters.NumberOfWaits)
    {
        var id = searchContext.Id;
        var name = searchContext.Name;
        var controlType = searchContext.ControlType;
        var className = searchContext.ClassName;
        var isRaw = searchContext.IsRaw;

        var conditions = new List<Condition>();
        if (!string.IsNullOrWhiteSpace(id))
        {
            conditions.Add(new PropertyCondition(AutomationElement.AutomationIdProperty, id));
        }

        if (!string.IsNullOrWhiteSpace(name))
        {
            conditions.Add(new PropertyCondition(AutomationElement.NameProperty, name));
        }

        if (!string.IsNullOrWhiteSpace(className))
        {
            conditions.Add(new PropertyCondition(AutomationElement.ClassNameProperty, className));
        }

        if (controlType is not null)
        {
            conditions.Add(new PropertyCondition(AutomationElement.ControlTypeProperty, controlType));
        }

        if (isRaw)
        {
            conditions.Add(System.Windows.Automation.Automation.RawViewCondition);
        }

        if (!conditions.Any())
        {
            return Array.Empty<AutomationElement>();
        }

        var resultCondition = conditions.Count == 1
            ? conditions[0]
            : new AndCondition(conditions.ToArray());

        return FindAll(element, resultCondition, scope, numberOfWaits);
    }

    public static IEnumerable<AutomationElement> FindAll(this AutomationElement element, Condition condition, TreeScope scope = TreeScope.Descendants, int numberOfWaits = SearchParameters.NumberOfWaits)
    {
        ArgumentNullException.ThrowIfNull(element);

        var numWaits = 0;

        IEnumerable<AutomationElement> foundElements;

        do
        {
            foundElements = element.TryFindElementsByCondition(scope, condition);

            ++numWaits;
            Thread.Sleep(100);
        } 
        while (foundElements is null && numWaits < numberOfWaits);

        return foundElements ?? Array.Empty<AutomationElement>();
    }

    private static IEnumerable<AutomationElement> TryFindElementsByCondition(this AutomationElement element, TreeScope scope, Condition condition)
    {
        if (scope == TreeScope.Parent)
        {
            var parent = element.GetParent(condition);
            return parent is not null ? new List<AutomationElement> { parent } : Array.Empty<AutomationElement>();
        }

        if (scope == TreeScope.Ancestors)
        {
            return element.GetAncestors(condition).ToList();
        }

        return element.FindAll(scope, condition).OfType<AutomationElement>();
    }
}