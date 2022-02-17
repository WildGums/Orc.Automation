namespace Orc.Automation.Controls
{
    using System;
    using System.Windows.Automation;

    [AutomatedControl(ControlTypeName = nameof(ControlType.Window))]
    public class Window : FrameworkElement
    {
        public Window(AutomationElement element)
            : base(element, ControlType.Window)
        {
            Automation.AddAutomationEventHandler(WindowPattern.WindowOpenedEvent, Element, TreeScope.Subtree, OnDialogOpened);
        }

        private void OnDialogOpened(object sender, AutomationEventArgs e)
        {
            DialogOpened?.Invoke(sender, e);
        }

        /// <summary>
        /// Close window
        /// </summary>
        public void Close() => Element.CloseWindow();

        public event EventHandler<AutomationEventArgs> DialogOpened;
    }
}
