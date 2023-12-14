namespace Orc.Automation;

using System;
using System.Linq;

public abstract class ExtensionAutomationFuncBase<TOwner, TResult, TValue1, TValue2, TValue3, TValue4, TValue5> : ExtensionAutomationFuncBase<TOwner, TResult>
    where TOwner : System.Windows.FrameworkElement
{
    protected override TResult Invoke(TOwner owner, params object?[] parameters)
    {
        var value1 = parameters.Extract<TValue1>(0);
        var value2 = parameters.Extract<TValue2>(1);
        var value3 = parameters.Extract<TValue3>(2);
        var value4 = parameters.Extract<TValue4>(3);
        var value5 = parameters.Extract<TValue5>(4);

        return Invoke(owner, value1, value2, value3, value4, value5);
    }

    protected abstract TResult Invoke(TOwner owner, TValue1 value1, TValue2 value2, TValue3 value3, TValue4 value4, TValue5 value5);
}

public abstract class ExtensionAutomationFuncBase<TOwner, TResult, TValue1, TValue2, TValue3, TValue4> : ExtensionAutomationFuncBase<TOwner, TResult>
    where TOwner : System.Windows.FrameworkElement
{
    protected override TResult Invoke(TOwner owner, params object?[] parameters)
    {
        var value1 = parameters.Extract<TValue1>(0);
        var value2 = parameters.Extract<TValue2>(1);
        var value3 = parameters.Extract<TValue3>(2);
        var value4 = parameters.Extract<TValue4>(3);

        return Invoke(owner, value1, value2, value3, value4);
    }

    protected abstract TResult Invoke(TOwner owner, TValue1 value1, TValue2 value2, TValue3 value3, TValue4 value4);
}

public abstract class ExtensionAutomationFuncBase<TOwner, TResult, TValue1, TValue2, TValue3> : ExtensionAutomationFuncBase<TOwner, TResult>
    where TOwner : System.Windows.FrameworkElement
{
    protected override TResult Invoke(TOwner owner, params object?[] parameters)
    {
        var value1 = parameters.Extract<TValue1>(0);
        var value2 = parameters.Extract<TValue2>(1);
        var value3 = parameters.Extract<TValue3>(2);

        return Invoke(owner, value1, value2, value3);
    }

    protected abstract TResult Invoke(TOwner owner, TValue1 value1, TValue2 value2, TValue3 value3);
}

public abstract class ExtensionAutomationFuncBase<TOwner, TResult, TValue1, TValue2> : ExtensionAutomationFuncBase<TOwner, TResult>
    where TOwner : System.Windows.FrameworkElement
{
    protected override TResult Invoke(TOwner owner, params object?[] parameters)
    {
        var value1 = parameters.Extract<TValue1>(0);
        var value2 = parameters.Extract<TValue2>(1);

        return Invoke(owner, value1, value2);
    }

    protected abstract TResult Invoke(TOwner owner, TValue1 value1, TValue2 value2);
}

public abstract class ExtensionAutomationFuncBase<TOwner, TResult, TValue> : ExtensionAutomationFuncBase<TOwner, TResult>
    where TOwner : System.Windows.FrameworkElement
{
    protected override TResult Invoke(TOwner owner, params object?[] parameters)
    {
        var value = parameters.Extract<TValue>(0);

        return Invoke(owner, value);
    }

    protected abstract TResult Invoke(TOwner owner, TValue value);
}
    
public abstract class ExtensionAutomationFuncBase<TOwner, TResult> : ExtensionAutomationMethodBase<TOwner>
    where TOwner : System.Windows.FrameworkElement
{
    protected override bool TryInvoke(TOwner owner, AutomationMethod method, out AutomationValue result)
    {
        var automationInputParameters = method.Parameters?
                                            .Select(x => x?.ExtractValue())
                                            .ToArray()
                                        ?? Array.Empty<object>();

        var tResult = Invoke(owner, automationInputParameters);

        result = AutomationValue.FromValue(tResult, typeof(TResult));

        return true;
    }

    protected abstract TResult Invoke(TOwner owner, params object?[] parameters);
}