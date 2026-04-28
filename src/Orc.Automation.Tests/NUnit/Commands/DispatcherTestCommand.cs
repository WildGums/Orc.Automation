namespace Orc.Automation.Tests;

using Catel.Services;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Commands;

public class DispatcherTestCommand : DelegatingTestCommand
{
    public DispatcherTestCommand(TestCommand innerCommand)
        : base(innerCommand)
    {
    }

    public override TestResult Execute(TestExecutionContext context)
    {
        var dispatcherService = new DispatcherService(NullLogger<DispatcherService>.Instance, 
            new DispatcherProviderService(NullLogger<DispatcherProviderService>.Instance));

        dispatcherService.Invoke(() => { context.CurrentResult = innerCommand.Execute(context); });

        return context.CurrentResult;
    }
}
