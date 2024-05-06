namespace Orc.Automation.Tests;

using System;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class TestCaseSourceAttribute<T1> : TestCaseSourceGenericAttribute
{
    public TestCaseSourceAttribute(string sourceName)
        : base(sourceName) => TypeArguments = new[]
    {
        typeof(T1)
    };
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class TestCaseSourceAttribute<T1, T2> : TestCaseSourceGenericAttribute
{
    public TestCaseSourceAttribute(string sourceName)
        : base(sourceName) => TypeArguments = new[]
    {
        typeof(T1),
        typeof(T2)
    };
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class TestCaseSourceAttribute<T1, T2, T3> : TestCaseSourceGenericAttribute
{
    public TestCaseSourceAttribute(string sourceName)
        : base(sourceName) => TypeArguments = new[]
    {
        typeof(T1),
        typeof(T2),
        typeof(T3)
    };
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class TestCaseSourceAttribute<T1, T2, T3, T4> : TestCaseSourceGenericAttribute
{
    public TestCaseSourceAttribute(string sourceName)
        : base(sourceName) => TypeArguments = new[]
    {
        typeof(T1),
        typeof(T2),
        typeof(T3),
        typeof(T4)
    };
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class TestCaseSourceAttribute<T1, T2, T3, T4, T5> : TestCaseSourceGenericAttribute
{
    public TestCaseSourceAttribute(string sourceName)
        : base(sourceName) => TypeArguments = new[]
    {
        typeof(T1),
        typeof(T2),
        typeof(T3),
        typeof(T4),
        typeof(T5)
    };
}