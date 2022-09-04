namespace Orc.Automation.Controls
{
    using System.Collections.Generic;
    using System.Windows.Automation;

    [Control(ControlTypeName = nameof(ControlType.Edit))]
    public class Edit : FrameworkElement<EditModel>
    {
        public Edit(AutomationElement element) 
            : base(element, ControlType.Edit)
        {
        }

        public string Text
        {
            get => Element.GetValue<string>();
            set => Element.SetValue(value);
        }

        public IReadOnlyList<string> SelectedTextRanges
        {
            get => Element.GetSelectedTextRanges();
        }
    }
}
