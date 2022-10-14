namespace Orc.Automation.Controls
{
    using System;
    using System.Windows.Automation;
    using Catel.IoC;
    using Catel.Reflection;
    using Services;

    [Control(ControlTypeName = nameof(ControlType.Window))]
    public class Window : Window<WindowModel>
    {
        public static AutomationElement MainWindow => ServiceLocator.Default.ResolveType<ISetupAutomationService>()?.CurrentSetup
            ?.MainWindow;

        public static TWindow WaitForWindow<TWindow>(string? id = null, string? name = null, int numberOfWaits = 10)
            where TWindow : AutomationControl, IWindow
        {
            var window = MainWindow?.Find<TWindow>(id: id, name: name, numberOfWaits: numberOfWaits);

            return window;
        }

        public Window(AutomationElement element)
            : base(element)
        {
        }
    }

    public abstract class Window<TModel, TMap> : Window<TModel>
        where TModel : WindowModel
        where TMap : AutomationBase
    {
        public Window(AutomationElement element)
            : base(element)
        {
        }

        protected TMap Map => Map<TMap>();

        protected TValue? GetMapValue<TValue>(object? source, string propertyName)
        {
            return Map.GetMapValue<TValue>(source, propertyName);
        }

        protected void SetMapValue<TValue>(object? source, string propertyName, TValue value)
        {
            Map.SetMapValue<TValue>(source, propertyName, value);
        }
    }

    public abstract class Window<TModel> : FrameworkElement<TModel>, IWindow
        where TModel : WindowModel
    {
        public Window(AutomationElement element)
            : base(element, ControlType.Window)
        {
            Automation.AddAutomationEventHandler(WindowPattern.WindowOpenedEvent, Element, TreeScope.Subtree, OnDialogOpened);
        }

        private void OnDialogOpened(object? sender, AutomationEventArgs e)
        {
            DialogOpened?.Invoke(sender, e);
        }

        /// <summary>
        /// Close window
        /// </summary>
        public virtual void Close() => Element.CloseWindow();

        public event EventHandler<AutomationEventArgs>? DialogOpened;
    }

    public interface IWindow
    {
        void Close();
        event EventHandler<AutomationEventArgs>? DialogOpened;
    }
}
