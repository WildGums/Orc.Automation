namespace Orc.Automation.Controls
{
    using System.Windows;
    using System.Windows.Automation;
    using System.Windows.Media;

    public class FrameworkElement : AutomationControl
    {
        public FrameworkElement(AutomationElement element, ControlType controlType)
            : base(element, controlType)
        {
        }

        public FrameworkElement(AutomationElement element) 
            : base(element)
        {
        }

        public double Height
        {
            get => Access.GetValue<double>();
            set => Access.SetValue(value);
        }

        public double Width
        {
            get => Access.GetValue<double>();
            set => Access.SetValue(value);
        }

        public SolidColorBrush Background
        {
            get => Access.GetValue<SolidColorBrush>();
            set => Access.SetValue(value);
        }

        public SolidColorBrush BorderBrush
        {
            get => Access.GetValue<SolidColorBrush>();
            set => Access.SetValue(value);
        }

        public HorizontalAlignment HorizontalAlignment
        {
            get => Access.GetValue<HorizontalAlignment>();
            set => Access.SetValue(value);
        }

        public VerticalAlignment VerticalAlignment
        {
            get => Access.GetValue<VerticalAlignment>();
            set => Access.SetValue(value);
        }

        //public SolidColorBrush Foreground
        //{
        //    get => Access.GetValue<SolidColorBrush>();
        //    set => Access.SetValue(value);
        //}

        public double ActualWidth
        {
            get => Access.GetValue<double>();
        }

        public double ActualHeight
        {
            get => Access.GetValue<double>();
        }

        public bool Focusable
        {
            get => Access.GetValue<bool>();
            set => Access.SetValue(value);
        }


    }
}
