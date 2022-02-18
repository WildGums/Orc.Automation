namespace Orc.Automation
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Catel.Reflection;

    public class TestHostAutomationPeer : RunMethodAutomationPeerBase
    {
        private readonly TestHost _testHost;

        public TestHostAutomationPeer(TestHost owner) 
            : base(owner)
        {
            _testHost = owner;
        }

        [AutomationMethod]
        public string PutControl(string controlTypeFullName)
        {
            return _testHost.PutControl(controlTypeFullName);
        }

        [AutomationMethod]
        public void ClearControls()
        {
            _testHost.ClearControls();
        }

        [AutomationMethod]
        public bool LoadResources(string uri)
        {
            return _testHost.LoadResources(uri);
        }

        [AutomationMethod]
        public bool LoadAssembly(string location)
        {
            var assembly = _testHost.LoadAssembly(location);

            return assembly is not null;
        }

        [AutomationMethod]
        public void LoadUnmanaged(string location)
        {
            _testHost.LoadUnmanaged(location);
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
                method.Invoke(null, new object[]{"Default"});
            }
            catch (Exception ex)
            {
                return $"Fail {ex.Message}";
            }

            return "Success";
        }
    }
}
