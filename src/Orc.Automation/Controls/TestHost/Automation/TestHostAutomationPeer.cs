namespace Orc.Automation
{
    using System;
    using System.Linq;

    public class TestHostAutomationPeer : AutomationControlPeerBase<TestHost>
    {
        public TestHostAutomationPeer(TestHost owner) 
            : base(owner)
        {
            
        }

        [AutomationMethod]
        public string PutControl(string controlTypeFullName)
        {
            return Control.PutControl(controlTypeFullName);
        }

        [AutomationMethod]
        public void ClearControls()
        {
            Control.ClearControls();
        }

        [AutomationMethod]
        public bool LoadResources(string uri)
        {
            return Control.LoadResources(uri);
        }

        [AutomationMethod]
        public bool LoadAssembly(string location)
        {
            var assembly = Control.LoadAssembly(location);

            return assembly is not null;
        }

        [AutomationMethod]
        public void LoadUnmanaged(string location)
        {
            Control.LoadUnmanaged(location);
        }

        [AutomationMethod]
        public object RunMethod(Type type, string methodName)
        {
            var method = type.GetMethods().FirstOrDefault(x => x.Name == methodName);
            if (method is null)
            {
                return $"There is no such method with name '{methodName}' in type '{type.FullName}'";
            }

            try
            {
                method.Invoke(null, new object?[]{"Default"});
            }
            catch (Exception ex)
            {
                return $"Fail {ex.Message}";
            }

            return "Success";
        }
    }
}
