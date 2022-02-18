namespace Orc.Automation.Host.Views
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using Catel.Windows;

    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            CanCloseUsingEscape = false;
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

                if (_countClick == 1)
                {
                    var elementStr = automationMethod.ToString();

                    testHostPeer.SetValue(elementStr);
                    testHostPeer.Invoke();

                    var value = testHostPeer.Value;
                }
                else
                {
                    var filterBox = TestHost.GetChildren().ElementAt(0).GetChildren().ElementAt(0);
                        

                    var filterBoxPeerType = TypeHelper.GetTypeByName("Orc.Controls.Automation.FilterBoxAutomationPeer");
                    var automationPeer = Activator.CreateInstance(filterBoxPeerType, filterBox); //new FrameworkElementAutomationPeer(culturePicker);

                    dynamic automationPeerDyn = automationPeer;

                    automationPeerDyn.SetValue(File.ReadAllText("C:\\Temp\\Temp.txt"));
                    automationPeerDyn.Invoke();

                    break;



                    //automationPeer

                    //testHostPeer.SetValue(File.ReadAllText("C:\\Temp\\TestBeh.txt"));

                    //testHostPeer.Invoke();

                    //if (i == 14)
                    //{
                    //    var elementStr = automationMethod.ToString();

                    //    testHostPeer.SetValue(elementStr);
                    //    testHostPeer.Invoke();
                    //    var value = testHostPeer.Value;
                    //}
                }
            }


            //Do some temporary test stuff here
        }
    }
}
