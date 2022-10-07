namespace Orc.Automation
{
    using System;
    using System.Linq;

    public abstract class ExtensionAutomationActionBase<TOwner, TValue1, TValue2, TValue3, TValue4, TValue5> : ExtensionAutomationActionBase<TOwner>
        where TOwner : System.Windows.FrameworkElement
    {
        protected override void Invoke(TOwner owner, params object?[] parameters)
        {
            var value1 = parameters.Extract<TValue1>(0);
            var value2 = parameters.Extract<TValue2>(1);
            var value3 = parameters.Extract<TValue3>(2);
            var value4 = parameters.Extract<TValue4>(3);
            var value5 = parameters.Extract<TValue5>(3);

            Invoke(owner, value1, value2, value3, value4, value5);
        }

        protected abstract void Invoke(TOwner owner, TValue1 inputValue1, TValue2 inputValue2, TValue3 inputValue3, TValue4 inputValue4, TValue5 inputValue5);
    }

    public abstract class ExtensionAutomationActionBase<TOwner, TValue1, TValue2, TValue3, TValue4> : ExtensionAutomationActionBase<TOwner>
        where TOwner : System.Windows.FrameworkElement
    {
        protected override void Invoke(TOwner owner, params object?[] parameters)
        {
            var value1 = parameters.Extract<TValue1>(0);
            var value2 = parameters.Extract<TValue2>(1);
            var value3 = parameters.Extract<TValue3>(2);
            var value4 = parameters.Extract<TValue4>(3);

            Invoke(owner, value1, value2, value3, value4);
        }

        protected abstract void Invoke(TOwner owner, TValue1 inputValue1, TValue2 inputValue2, TValue3 inputValue3, TValue4 inputValue4);
    }

    public abstract class ExtensionAutomationActionBase<TOwner, TValue1, TValue2, TValue3> : ExtensionAutomationActionBase<TOwner>
        where TOwner : System.Windows.FrameworkElement
    {
        protected override void Invoke(TOwner owner, params object?[] parameters)
        {
            var value1 = parameters.Extract<TValue1>(0);
            var value2 = parameters.Extract<TValue2>(1);
            var value3 = parameters.Extract<TValue3>(2);

            Invoke(owner, value1, value2, value3);
        }

        protected abstract void Invoke(TOwner owner, TValue1 inputValue1, TValue2 inputValue2, TValue3 inputValue3);
    }

    public abstract class ExtensionAutomationActionBase<TOwner, TValue1, TValue2> : ExtensionAutomationActionBase<TOwner>
        where TOwner : System.Windows.FrameworkElement
    {
        protected override void Invoke(TOwner owner, params object?[] parameters)
        {
            var value1 = parameters.Extract<TValue1>(0);
            var value2 = parameters.Extract<TValue2>(1);

            Invoke(owner, value1, value2);
        }

        protected abstract void Invoke(TOwner owner, TValue1 inputValue1, TValue2 inputValue2);
    }
    
    public abstract class ExtensionAutomationActionBase<TOwner, TValue> : ExtensionAutomationActionBase<TOwner>
        where TOwner : System.Windows.FrameworkElement
    {
        protected override void Invoke(TOwner owner, params object?[] parameters)
        {
            var value = parameters.Extract<TValue>(0);

            Invoke(owner, value);
        }

        protected abstract void Invoke(TOwner owner, TValue inputValue1);
    }
    
    public abstract class ExtensionAutomationActionBase<TOwner> : ExtensionAutomationMethodBase<TOwner>
        where TOwner : System.Windows.FrameworkElement
    {
        protected override bool TryInvoke(TOwner owner, AutomationMethod method, out AutomationValue result)
        {
            var automationInputParameters = method.Parameters?
                .Select(x => x?.ExtractValue())
                .ToArray()
                ?? Array.Empty<object>();

            Invoke(owner, automationInputParameters);

            //Dummy value
            result = AutomationValue.FromValue(true);

            return true;
        }

        protected abstract void Invoke(TOwner owner, params object?[] parameters);
    }
}
