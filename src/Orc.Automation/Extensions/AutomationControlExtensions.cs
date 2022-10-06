namespace Orc.Automation
{
    using System;
    using System.Windows;
    using System.Windows.Automation;
    using System.Windows.Input;
    using Catel;
    using Catel.IoC;

    public static class AutomationControlExtensions
    {
        public static void DragAndDrop(this AutomationControl control, Point destinationPoint)
        {
            control.MouseHover();

            Wait.UntilResponsive(500);

            MouseInput.Down();

            Wait.UntilResponsive(500);

            MouseInput.MoveTo(destinationPoint);

            Wait.UntilResponsive(500);

            MouseInput.Up();
        }

        public static void DragAndDrop(this AutomationControl control, double deltaX, double deltaY)
        {
            control.MouseHover();

            Wait.UntilResponsive(500);

            MouseInput.Down();

            Wait.UntilResponsive(500);

            MouseInput.Move(deltaX, deltaY);

            Wait.UntilResponsive(500);

            MouseInput.Up();
        }

        public static TAutomationControl As<TAutomationControl>(this AutomationControl control)
        {
            ArgumentNullException.ThrowIfNull(control);

#pragma warning disable IDISP004 // Don't ignore created IDisposable
            return control.GetTypeFactory().CreateInstanceWithParametersAndAutoCompletion<TAutomationControl>(control.Element);
#pragma warning restore IDISP004 // Don't ignore created IDisposable
        }

        public static void MouseHover(this AutomationControl control)
        {
            ArgumentNullException.ThrowIfNull(control);

            var rect = control.BoundingRectangle;

            MouseInput.MoveTo(rect.GetClickablePoint());
        }

        public static void MouseOut(this AutomationControl control)
        {
            ArgumentNullException.ThrowIfNull(control);

            var rect = control.BoundingRectangle;

            var pointOut = rect.GetPointOut();

            MouseInput.MoveTo(pointOut);
        }

        public static void MouseClick(this AutomationControl control, MouseButton mouseButton = MouseButton.Left)
        {
            control.Element.MouseClick(mouseButton);
        }

        public static object GetLeftTop(this AutomationControl control)
        {
            ArgumentNullException.ThrowIfNull(control);

            return control.Execute<GetLeftTopMethodRun>();
        }

        public static AutomationElement Find(this AutomationControl parent, string id = null, string name = null, string className = null, bool isRaw = false, ControlType controlType = null, TreeScope scope = TreeScope.Descendants, int numberOfWaits = SearchParameters.NumberOfWaits)
        {
            ArgumentNullException.ThrowIfNull(parent);

            return parent.Element.Find(id, name, className, isRaw, controlType, scope, numberOfWaits);
        }

        public static TElement Find<TElement>(this AutomationControl parent, string id = null, string name = null, string className = null, bool isRaw = false, ControlType controlType = null, TreeScope scope = TreeScope.Descendants, int numberOfWaits = SearchParameters.NumberOfWaits)
            where TElement : AutomationControl
        {
            ArgumentNullException.ThrowIfNull(parent);

            return parent.Element.Find<TElement>(id, name, className, isRaw, controlType, scope, numberOfWaits);
        }
    }
}
