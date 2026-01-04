namespace Orc.Automation.Tests;

using System;
using System.Windows.Automation;
using Catel;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Orc.Serialization.Json;
using Services;

public abstract class UiTestsBase : IDisposable
{
    private ServiceProvider? _serviceProvider;
    private ISetupAutomationService _setupAutomationService;
    private bool _disposed;

    protected UiTestsBase()
    {
    }

    protected AutomationSetup Setup { get; private set; }

    protected virtual string ExecutablePath => string.Empty;
    protected virtual string Args => null;
    protected virtual string MainWindowAutomationId => string.Empty;

    protected virtual Condition FindMainWindowCondition =>
        new PropertyCondition(AutomationElement.AutomationIdProperty, MainWindowAutomationId);

    protected ISetupAutomationService SetupAutomationService =>
        _setupAutomationService ??= CreateSetupAutomationService();

    public virtual void SetUp()
    {
#pragma warning disable IDISP003 // Don't ignore created IDisposable.
        Setup = SetupAutomationService?.Setup(ExecutablePath, FindMainWindowCondition, Args);
#pragma warning disable IDISP003 // Don't ignore created IDisposable.

        Assert.That(Setup, Is.Not.Null);
    }

    public virtual void TearDown()
    {
        Dispose();
    }

    public virtual void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        Setup?.Dispose();
        Setup = null;

        _serviceProvider?.Dispose();
        _serviceProvider = null;

        _disposed = true;
    }

    protected virtual IServiceProvider GetServiceProvider()
    {
        var serviceProvider = _serviceProvider;
        if (serviceProvider is null)
        {
            var serviceCollection = new ServiceCollection();

            ConfigureServiceCollection(serviceCollection);

            serviceProvider = _serviceProvider = serviceCollection.BuildServiceProvider();

            serviceProvider.CreateTypesThatMustBeConstructedAtStartup();
        }

        return serviceProvider;
    }

    protected virtual void ConfigureServiceCollection(IServiceCollection serviceCollection)
    {
        serviceCollection.AddCatelCore();
        serviceCollection.AddCatelMvvm();
        serviceCollection.AddOrcAutomation();
        serviceCollection.AddOrcAutomationTests();
        serviceCollection.AddOrcSerializationJson();
    }

    protected virtual ISetupAutomationService CreateSetupAutomationService()
    {
        return GetServiceProvider().GetRequiredService<ISetupAutomationService>();
    }

    protected virtual void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(GetType().FullName);
        }
    }
}
