namespace Orc.Automation
{
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
    }
}
