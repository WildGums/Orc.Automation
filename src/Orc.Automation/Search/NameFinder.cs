namespace Orc.Automation;

using System;
using System.Windows;

public class NameFinder : ConditionalPartFinderBase
{
    public NameFinder()
    {
        Name = string.Empty;
    }

    public static NameFinder Create(string name) => new()
    {
        Name = name
    };

    public string Name { get; set; }

    protected override bool IsMatch(object descendant)
    {
        ArgumentNullException.ThrowIfNull(descendant);

        return Equals(((DependencyObject)descendant).GetValue(FrameworkElement.NameProperty), Name);
    }
}