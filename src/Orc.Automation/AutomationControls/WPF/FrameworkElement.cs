namespace Orc.Automation.Controls
{
    using System.Windows;
    using System.Windows.Automation;

    public class FrameworkElement<TControlModel, TMap> : AutomationControl<TControlModel, TMap>
        where TControlModel : FrameworkElementModel
        where TMap : AutomationBase
    {
        public FrameworkElement(AutomationElement element, ControlType controlType) 
            : base(element, controlType)
        {
        }

        public FrameworkElement(AutomationElement element) 
            : base(element)
        {
            
        }
    }

    public class FrameworkElement<TControlModel> : AutomationControl<TControlModel>
        where TControlModel : FrameworkElementModel
    {
        public FrameworkElement(AutomationElement element, ControlType controlType) 
            : base(element, controlType)
        {
        }

        public FrameworkElement(AutomationElement element) 
            : base(element)
        {
        }
    }

    public class FrameworkElement : AutomationControl<FrameworkElementModel>
    {
        public FrameworkElement(AutomationElement element, ControlType controlType)
            : base(element, controlType)
        {
        }

        public FrameworkElement(AutomationElement element) 
            : base(element)
        {
        }

        public override Rect BoundingRectangle
        {
            get
            {
                if (IsPart)
                {
                    var current = Current;

                    var dpi = DpiHelper.GetDpi();

                    var leftTop = (Point) this.GetLeftTop();
                    var size = new Size(current.ActualWidth * dpi, current.ActualHeight * dpi);

                    return new Rect(leftTop, size);
                }

                return base.BoundingRectangle;
            }
        }
    }
}
