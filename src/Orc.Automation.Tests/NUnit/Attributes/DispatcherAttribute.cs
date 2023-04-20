namespace Orc.Automation.Tests;

using System;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal.Commands;

[AttributeUsage(AttributeTargets.Method)]
public class DispatcherAttribute : NUnitAttribute, IWrapSetUpTearDown
{
    public TestCommand Wrap(TestCommand command)
    {
        return new DispatcherTestCommand(command);
    }
}
