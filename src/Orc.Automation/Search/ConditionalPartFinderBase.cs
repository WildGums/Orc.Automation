namespace Orc.Automation;

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

public abstract class ConditionalPartFinderBase : IPartFinder
{
    protected abstract bool IsMatch(object descendant);

    public FrameworkElement? Find(FrameworkElement parent)
    {
        ArgumentNullException.ThrowIfNull(parent);

        return FindVisualDescendant(parent, IsMatch) as FrameworkElement;
    }

    private static DependencyObject? FindVisualDescendant(DependencyObject startElement, Predicate<object> condition)
    {
        ArgumentNullException.ThrowIfNull(startElement);
        ArgumentNullException.ThrowIfNull(condition);

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

            if (startElement is Decorator startElementAsDecorator)
            {
                return FindVisualDescendant(startElementAsDecorator.Child, condition);
            }

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
