namespace Orc.Automation.Host.Views
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Threading;
    using Catel.IoC;
    using Catel.Windows;

    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }



        private static int _countClick = 0;

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            //TestHost.LoadAssembly(@"C:\Source\Orc.Controls\output\Debug\Orc.Controls.Tests\net6.0-windows\DiffEngine.dll");
            //TestHost.LoadAssembly(@"C:\Source\Orc.Controls\output\Debug\Orc.Controls.Tests\net6.0-windows\ApprovalUtilities.dll");
            //TestHost.LoadAssembly(@"C:\Source\Orc.Controls\output\Debug\Orc.Controls.Tests\net6.0-windows\ApprovalTests.dll");
            //TestHost.LoadAssembly(@"C:\Source\Orc.Controls\output\Debug\Orc.Controls.Tests\net6.0-windows\Orc.Controls.dll");
            //TestHost.LoadAssembly(@"C:\Source\Orc.Controls\output\Debug\Orc.Controls.Tests\net6.0-windows\Orc.Automation.Tests.dll");

            //TestHost.LoadAssembly(@"C:\Source\Orc.Controls\output\Debug\Orc.Controls.Tests\net6.0-windows\Orc.Controls.Tests.dll");

            ////TestHost.LoadResources("pack://application:,,,/Orc.Controls;component/Themes/Generic.xaml");

            //var testHostPeer = new TestHostAutomationPeer(TestHost);

            //var background = testHostPeer.GetPropertyValue(TestHost, nameof(TestHost.Background));

            //try
            //{
            //    var typee = AppDomain.CurrentDomain.GetAssemblies()
            //        .SelectMany(x =>
            //        {
            //            try
            //            {
            //                if (x.FullName.Contains("Orc.Controls.Tests"))
            //                {
            //                    var types = x.GetTypes();

            //                    return types;
            //                }

            //                return x.GetTypes();
            //            }
            //            catch (Exception)
            //            {
            //                return Enumerable.Empty<Type>();
            //            }
            //        })
            //        .FirstOrDefault(x => string.Equals(x.FullName, "Orc.Controls.Tests.CreateStyleForwardersMethodRun"));
            //}
            //catch (Exception exception)
            //{
            //    Console.WriteLine(exception);
            //}

            _countClick++;



            //try
            //{
            //    var selectButton = TestHost.FindVisualDescendantWithAutomationId("SelectButton");
            //}
            //catch
            //{
            //    Console.Write("ss");
            //}



            // var type = TypeHelper.GetTypeByName("Orc.Controls.Tests.CreateStyleForwardersMethodRun");

            var testHostPeer = new TestHostAutomationPeer(TestHost);

           // testHostPeer.AddAutomationMethod(type);

          //  var styleHelper = TypeHelper.GetTypeByName("Orc.Theming.StyleHelper");

       //     testHostPeer.RunMethod(styleHelper, "CreateStyleForwardersForDefaultStyles");

            AutomationMethodsList.Load();
            var methods = AutomationMethodsList.Instance;

            var i = 0;

            foreach (var automationMethod in methods.Methods)
            {
                i++;

                if (i == 17)
                {

                }

                var elementStr = automationMethod.ToString();

                testHostPeer.SetValue(elementStr);
                testHostPeer.Invoke();

                DoEvents();

                //if (i == _countClick)
                //{
                //    if (_countClick == 16)
                //    {

                //    }

                //    if (_countClick is 2221 or 222 )
                //    {
                //        var control = TestHost.GetChildren().ElementAt(0).GetChildren().ElementAt(0);

                //        var controlPeerType = TypeHelper.GetTypeByName("Orc.FilterBuilder.Automation.EditFilterViewPeer");
                //        var controlPeer = Activator.CreateInstance(controlPeerType, control); //new FrameworkElementAutomationPeer(culturePicker);

                //        dynamic controlPeerDyn = controlPeer;

                //        var elementStr = automationMethod.ToString();
                //        controlPeerDyn.SetValue(elementStr);
                //        controlPeerDyn.Invoke();
                //    }
                //    else
                //    {
                //        var elementStr = automationMethod.ToString();

                //        testHostPeer.SetValue(elementStr);
                //        testHostPeer.Invoke();

                //        DoEvents();
                //    }
                //}
            }
            //Do some temporary test stuff here
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            var position = e.GetPosition(this);
            //vm.MouseXPosition = position.X;
          //  vm.MouseYPosition = position.Y;
        }

        public static void DoEvents()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Render,
                new Action(delegate { }));
        }
    }
}
