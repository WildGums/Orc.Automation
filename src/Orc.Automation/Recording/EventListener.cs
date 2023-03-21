namespace Orc.Automation.Recording;

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Windows;
using System.Windows.Input;

public static class ControlEventListener
{
    public static void Start()
    {
        /*subscribe to all valuable tree events*/

        //Loaded
#pragma warning disable WPF0092 // Use correct handler type
        EventManager.RegisterClassHandler(
            typeof(FrameworkElement),
            FrameworkElement.LoadedEvent,
            new LoadedEventHandler(OnLoaded),
            true
        );
#pragma warning restore WPF0092 // Use correct handler type
    }

    private static void OnLoaded(object? sender, LoadedEventArgs e)
    {
            
    }
}

public class InputEventEntry
{
    public InputEventEntry(InputEventArgs args)
    {
        ArgumentNullException.ThrowIfNull(args);

        Args = args;
        Timestamp = args.Timestamp;

        if (args.Source is FrameworkElement frameworkElement)
        {
            Name = frameworkElement.Name;
            ElementType = frameworkElement.GetType();
        }
    }

    public InputEventArgs Args { get; }
    public int Timestamp { get; }
    public string Name { get; }
    public Type? ElementType { get; }
}

public static class EventListener
{
    public static readonly List<InputEventEntry> Events = new ();
    public static Func<InputEventEntry, bool>? Filter;

    public static void StartListen()
    {
        EventManager.RegisterClassHandler(
            typeof(UIElement),
            UIElement.PreviewMouseDownEvent,
            new MouseButtonEventHandler(MouseDown),
            true
        );

        EventManager.RegisterClassHandler(
            typeof(UIElement),
            UIElement.PreviewMouseUpEvent,
            new MouseButtonEventHandler(MouseUp),
            true
        );

        EventManager.RegisterClassHandler(
            typeof(UIElement),
            UIElement.PreviewKeyDownEvent,
            new KeyEventHandler(KeyDown),
            true
        );

        EventManager.RegisterClassHandler(
            typeof(UIElement),
            UIElement.PreviewKeyUpEvent,
            new KeyEventHandler(KeyUp),
            true
        );

        EventManager.RegisterClassHandler(
            typeof(UIElement),
            UIElement.PreviewTextInputEvent,
            new TextCompositionEventHandler(TextInput),
            true
        );

        //EventManager.RegisterClassHandler(
        //    typeof(UIElement),
        //    Keyboard.GotKeyboardFocusEvent,
        //    new KeyboardFocusChangedEventHandler(OnKeyboardFocusChanged),
        //    true
        //);

        //EventManager.RegisterClassHandler(
        //    typeof(UIElement),
        //    Keyboard.LostKeyboardFocusEvent,
        //    new KeyboardFocusChangedEventHandler(OnKeyboardFocusChanged),
        //    true
        //);
    }

    public static void StopListen()
    {

    }

    private static void MouseDown(object? sender, MouseButtonEventArgs e)
    {
        AddEvent(e);
    }

    private static void MouseUp(object? sender, MouseButtonEventArgs e)
    {
        AddEvent(e);
    }

    private static void KeyDown(object? sender, KeyEventArgs e)
    {
        AddEvent(e);
    }

    private static void KeyUp(object? sender, KeyEventArgs e)
    {
        AddEvent(e);
    }

    private static void TextInput(object? sender, TextCompositionEventArgs e)
    {
        AddEvent(e);
    }

    private static void AddEvent(InputEventArgs args)
    {
        ArgumentNullException.ThrowIfNull(args);

        var entry = new InputEventEntry(args);

        var filter = Filter;
        if (filter is not null)
        {
            if (!filter.Invoke(entry))
            {
                return;
            }
        }

        Events.Add(entry);
    }
}