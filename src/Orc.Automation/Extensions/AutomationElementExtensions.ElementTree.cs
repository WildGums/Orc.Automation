namespace Orc.Automation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;
using Controls;

public partial class AutomationElementExtensions
{
    public static string? TryGetDisplayText(this AutomationElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        var rawTreeWalker = TreeWalker.RawViewWalker;
        var rawElement = rawTreeWalker.GetFirstChild(element);

        return rawElement?.Current.Name ?? element.Find<Text>(isRaw: true)?.Value;
    }

    public static IEnumerable<AutomationElement> GetAncestors(this AutomationElement element, Condition condition)
    {
        ArgumentNullException.ThrowIfNull(element);

        var treeWalker = new TreeWalker(condition);

        var parent = GetParent(element);

        while (parent is not null)
        {
            if (treeWalker.GetParent(element) is not null)
            {
                yield return parent;
            }

            parent = GetParent(parent);
        }
    }

    public static AutomationElement? GetParent(this AutomationElement element, Condition? condition = null)
    {
        ArgumentNullException.ThrowIfNull(element);

        return condition is null ? TreeWalker.ControlViewWalker.GetParent(element) : new TreeWalker(condition).GetParent(element);
    }

    public static AutomationElement? GetRawChild(this AutomationElement containerElement, int index)
    {
        ArgumentNullException.ThrowIfNull(containerElement);

        var item = TreeWalker.RawViewWalker.GetFirstChild(containerElement);
        var currentIndex = 0;

        while (!Equals(index, currentIndex) && item is not null)
        {
            item = TreeWalker.RawViewWalker.GetNextSibling(item);
            currentIndex++;
        }

        return item;
    }

    public static AutomationElement? GetChild(this AutomationElement containerElement, int index)
    {
        ArgumentNullException.ThrowIfNull(containerElement);

        var item = TreeWalker.ControlViewWalker.GetFirstChild(containerElement);
        var currentIndex = 0;

        while (!Equals(index, currentIndex) && item is not null)
        {
            item = TreeWalker.ControlViewWalker.GetNextSibling(item);
            currentIndex++;
        }

        return item;
    }

    public static AutomationElement? GetFirstDescendant(this AutomationElement element, Condition condition)
    {
        ArgumentNullException.ThrowIfNull(element);

        var treeWalker = new TreeWalker(condition);

        var item = treeWalker.GetFirstChild(element);

        return item;
    }

    public static IEnumerable<AutomationElement> GetChildElements(this AutomationElement containerElement)
    {
        ArgumentNullException.ThrowIfNull(containerElement);

        var item = TreeWalker.ControlViewWalker.GetFirstChild(containerElement);

        while (item is not null)
        {
            yield return item;

            item = TreeWalker.ControlViewWalker.GetNextSibling(item);
        }
    }

    public static IEnumerable<AutomationElement> GetDescendants(this AutomationElement containerElement)
    {
        ArgumentNullException.ThrowIfNull(containerElement);

        var children = containerElement.GetChildElements().ToList();
        while (children.Any())
        {
            var currentChild = children[0];
            children.RemoveAt(0);

            yield return currentChild;

            foreach (var currentChildChild in currentChild.GetChildElements())
            {
                children.Add(currentChildChild);
            }
        }
    }
}
