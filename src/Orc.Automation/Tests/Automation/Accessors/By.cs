namespace Orc.Automation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;
using Automation;
using Controls;

public class By
{
    private readonly AutomationElement _element;
    private readonly SearchContext _searchContext = new();
    private readonly Tab _tab;

    private int? _tabIndex;
    private TreeScope _treeScope = TreeScope.Descendants;

    public AutomationElement Element => _element;

    public By(AutomationElement element, Tab tab)
        : this(element)
    {
        ArgumentNullException.ThrowIfNull(tab);

        _tab = tab;
    }

    public By(AutomationElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        _element = element;
    }

    public By Raw()
    {
        _searchContext.IsRaw = true;

        return this;
    }

    public By Id(string id)
    {
        _searchContext.Id = id;

        return this;
    }

    public By Name(string name)
    {
        _searchContext.Name = name;

        return this;
    }

    public By ControlType(ControlType controlType)
    {
        _searchContext.ControlType = controlType;

        return this;
    }

    public By ClassName(string className)
    {
        _searchContext.ClassName = className;

        return this;
    }

    public By Condition(Condition condition)
    {
        _searchContext.Condition = condition;

        return this;
    }

    public By Tab(int tabIndex)
    {
        _tabIndex = tabIndex;

        return this;
    }

    public By Scope(TreeScope scope)
    {
        _treeScope = scope;

        return this;
    }

    public virtual AutomationElement? One()
    {
        if (_tabIndex is not null && _tab is not null)
        {
            return _tab.InTab(_tabIndex.Value, () => _element.Find(_searchContext, _treeScope));
        }

        return _element.Find(_searchContext, _treeScope);
    }

    public T Part<T>()
        where T : AutomationControl
    {
        var finder = SearchContextFinder.Create(_searchContext);
        var tempFrameworkElement = new FrameworkElement(_element);

        return tempFrameworkElement.Part<T>(finder);
    }

    public virtual IList<AutomationElement> Many()
    {
        if (_tabIndex is not null && _tab is not null)
        {
            return _tab.InTab(_tabIndex.Value, () => _element.FindAll(_searchContext, _treeScope)?.ToList());
        }

        return _element.FindAll(_searchContext, _treeScope)?.ToList();
    }
}