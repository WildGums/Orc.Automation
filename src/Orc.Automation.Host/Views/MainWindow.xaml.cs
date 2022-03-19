namespace Orc.Automation.Host.Views
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;
    using Theming;
    using ViewModels;

    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            CanCloseUsingEscape = false;

            ThemeManager.Current.SynchronizeTheme();
        }
        
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (ViewModel is not MainViewModel vm)
            {
                return;
            }

            var position = e.GetPosition(this);
            vm.MouseXPosition = position.X;
            vm.MouseYPosition = position.Y;
        }
    }
}
