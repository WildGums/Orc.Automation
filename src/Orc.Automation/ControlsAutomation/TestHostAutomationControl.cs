namespace Orc.Automation.Controls
{
    using System;
    using System.Windows.Automation;

    public class TestHostAutomationControl : AutomationControl
    {
        public TestHostAutomationControl(AutomationElement element) 
            : base(element)
        {
        }

        public object RunMethod(Type type, string methodName, params object[] parameters)
        {
            TryLoadAssembly(type.Assembly.Location);

            return Access.Execute<string>(nameof(TestHostAutomationPeer.RunMethod), type, methodName);
        }

        public void ClearControls()
        {
            Access.Execute(nameof(TestHostAutomationPeer.ClearControls));
        }

        public string PutControl(string controlFullName)
        {
            return Access.Execute<string>(nameof(TestHostAutomationPeer.PutControl), controlFullName);
        }

        public bool TryLoadResources(string resourceName)
        {
            return Access.Execute<bool>(nameof(TestHostAutomationPeer.LoadResources), resourceName);
        }

        public bool TryLoadAssembly(string assemblyLocation)
        {
            return Access.Execute<bool>(nameof(TestHostAutomationPeer.LoadAssembly), assemblyLocation);
        }

        public bool TryLoadUnmanaged(string assemblyLocation)
        {
            try
            {
                Access.Execute(nameof(TestHostAutomationPeer.LoadUnmanaged), assemblyLocation);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return false;
            }

            return true;
        }
    }
}
