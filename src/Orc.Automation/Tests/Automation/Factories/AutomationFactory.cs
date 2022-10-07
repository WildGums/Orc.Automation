namespace Orc.Automation
{
    using System;
    using Catel.IoC;

    public class AutomationFactory
    {
        public T? Create<T>(object element)
            where T : AutomationBase
        {
#pragma warning disable IDISP001 // Dispose created
            var typeFactory = this.GetTypeFactory();
#pragma warning restore IDISP001 // Dispose created

            if (element is AutomationControl control)
            {
                return typeFactory.CreateInstanceWithParametersAndAutoCompletion<T>(control)
                       ?? typeFactory.CreateInstanceWithParametersAndAutoCompletion<T>(control.Element);
            }

            return typeFactory.CreateInstanceWithParametersAndAutoCompletion<T>(element);
        }

        public T CreateRequired<T>(object element)
            where T : AutomationBase
        {
            var instance = Create<T>(element);
            if (instance is null)
            {
                throw new InvalidOperationException($"Cannot create required instance of '{typeof(T).Name}'");
            }

            return instance;
        }
    }
}
