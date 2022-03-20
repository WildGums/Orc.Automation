namespace Orc.Automation.Host.Views
{
    using System.Windows.Input;

    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        protected override void OnMouseMove(MouseEventArgs e)
        {
            var position = e.GetPosition(this);
            MouseXPositionRun.SetCurrentValue(System.Windows.Documents.Run.TextProperty, position.X.ToString());
            MouseYPositionRun.SetCurrentValue(System.Windows.Documents.Run.TextProperty, position.Y.ToString());
        }
    }
}
