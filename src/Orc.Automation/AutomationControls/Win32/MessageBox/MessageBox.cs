namespace Orc.Automation
{
    using System.Windows.Automation;
    using Controls;

    //TODO:Vladimir: check class name on other machines
    [AutomatedControl(ClassName = "#32770")]
    public class MessageBox : Window
    {
        public static void WaitYes() => Wait()?.Yes();
        public static void WaitNo() => Wait()?.Yes();
        public static void WaitOk() => Wait()?.Ok();
        public static void WaitCancel() => Wait()?.Cancel();

        public static MessageBox Wait() => Window.WaitForWindow<MessageBox>();


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
