namespace Orc.Automation
{
    using System.Windows.Automation;
    using Catel.IoC;
    using Services;

    //TODO:Vladimir: check class name on other machines
    [AutomatedControl(ClassName = "#32770")]
    public class MessageBox : AutomationControl
    {
        //TODO: Vladimir simplify? think of it
        public static MessageBox Current => ServiceLocator.Default.ResolveType<ISetupAutomationService>()?.CurrentSetup?.MainWindow?.Find<MessageBox>();

        public MessageBox(AutomationElement element) 
            : base(element)
        {
        }

        private MessageBoxMap Map => Map<MessageBoxMap>();

        public string Title => Element.Current.Name;
        public string Message => Map.ContentText.Value;

        public void Yes()
        {
            Map.YesButton?.Click();
        }

        public void No()
        {
            Map.NoButton?.Click();
        }

        public void Ok()
        {
            Map.OkButton?.Click();
        }

        public void Cancel()
        {
            Map.CancelButton?.Click();
        }
    }
}
