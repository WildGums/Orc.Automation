namespace Orc.Automation.Host.Views
{
    using System.Windows;
    using System.Windows.Input;
    using Microsoft.Win32;

    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void OnLoad(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                var filePath = openFileDialog.FileName;
                AutomationMethodsList.Load(filePath);
            }

            var testHost = TestHost;
            var testHostPeer = new TestHostAutomationPeer(testHost);

            foreach (var automationMethod in AutomationMethodsList.Instance.Methods)
            {
                var elementStr = automationMethod.ToString();
                testHostPeer.SetValue(elementStr);
                testHostPeer.Invoke();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            var position = e.GetPosition(this);
            MouseXPositionRun.SetCurrentValue(System.Windows.Documents.Run.TextProperty, position.X.ToString());
            MouseYPositionRun.SetCurrentValue(System.Windows.Documents.Run.TextProperty, position.Y.ToString());
        }
    }
}
