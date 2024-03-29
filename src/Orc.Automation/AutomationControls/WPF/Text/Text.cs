﻿namespace Orc.Automation.Controls;

using System.Windows.Automation;
using System.Windows.Media;

[Control(ControlTypeName = nameof(ControlType.Text))]
public class Text : FrameworkElement<TextModel>
{
    public Text(AutomationElement element) 
        : base(element, ControlType.Text)
    {
    }

    public SolidColorBrush Foreground
    {
        get => Access.GetValue<SolidColorBrush>();
        set => Access.SetValue(value);
    }

    public string Value => Element.Current.Name;
}