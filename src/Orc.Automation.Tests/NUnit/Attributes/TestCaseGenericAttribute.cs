#nullable enable
namespace Orc.Automation.Tests;

using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal.Builders;
using NUnit.Framework.Internal;
using System.Collections.Generic;
using System;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class TestCaseGenericAttribute : NUnit.Framework.TestCaseAttribute, ITestBuilder
{
    public TestCaseGenericAttribute(params object[] arguments)
        : base(arguments)
    {
    }

    public Type[]? TypeArguments { get; set; }

    IEnumerable<TestMethod> ITestBuilder.BuildFrom(IMethodInfo method, Test? suite)
    {
        if (!method.IsGenericMethodDefinition)
        {
            return base.BuildFrom(method, suite);
        }

        if (TypeArguments is null || TypeArguments.Length != method.GetGenericArguments().Length)
        {
            var parameters = new TestCaseParameters
            {
                RunState = RunState.NotRunnable
            };
            parameters.Properties.Set(PropertyNames.SkipReason, $"{nameof(TypeArguments)} should have {method.GetGenericArguments().Length} elements");
            return new[]
            {
                new NUnitTestCaseBuilder().BuildTestMethod(method, suite, parameters)
            };
        }

        var genMethod = method.MakeGenericMethod(TypeArguments);
        return base.BuildFrom(genMethod, suite);
    }
}
