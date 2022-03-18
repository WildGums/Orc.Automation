namespace Orc.Automation
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using Catel;

    public class ReflectionAutomationMethodRun : NamedAutomationMethodRun
    {
        private readonly AutomationControlPeerBase _peer;

        public ReflectionAutomationMethodRun(AutomationControlPeerBase peer, string methodName)
        {
            Argument.IsNotNull(() => peer);

            _peer = peer;
            Name = methodName;
        }

        public override string Name { get; }

        public override bool TryInvoke(FrameworkElement target, AutomationMethod automationMethod, out AutomationValue result)
        {
            var type = _peer.GetType();

            result = null;

            var method = type.GetMethod(Name);
            if (method is null)
            {
                return false;
            }

            var automationInputParameters = automationMethod.Parameters?
                    .Select(x => x?.ExtractValue())
                    .ToArray() 
                ?? Array.Empty<object>();

            var targetParameter = method.GetParameters().FirstOrDefault(x => x.GetCustomAttribute(typeof(TargetAttribute)) is not null);
            if (targetParameter is not null)
            {
                automationInputParameters = AttachTargetToParameters(target, automationMethod, automationInputParameters);
            }

            try
            {
                var methodResult = method.Invoke(_peer, automationInputParameters);
                result = AutomationValue.FromValue(methodResult);
            }
            catch (Exception ex)
            {
                File.AppendAllText("C:\\Temps\\ExceptionData.txt", ex.Message);
                File.AppendAllText("C:\\Temps\\ExceptionData.txt", "\r\n");
                File.AppendAllText("C:\\Temps\\ExceptionData.txt", ex.StackTrace);
                File.AppendAllText("C:\\Temps\\ExceptionData.txt", "\r\n");
                File.AppendAllText("C:\\Temps\\ExceptionData.txt", "\r\n");
                File.AppendAllText("C:\\Temps\\ExceptionData.txt", automationMethod.ToString());

                throw;
            }
            return true;
        }

        private static object[] AttachTargetToParameters(FrameworkElement target, AutomationMethod automationMethod, object[] automationInputParameters)
        {
            var tempParameters = automationInputParameters.ToList();

            tempParameters.Insert(0, target);

            automationInputParameters = tempParameters.ToArray();
            return automationInputParameters;
        }
    }
}
