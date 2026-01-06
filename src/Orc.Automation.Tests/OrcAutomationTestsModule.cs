namespace Orc
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Orc.Automation.Services;

    /// <summary>
    /// Core module which allows the registration of default services in the service collection.
    /// </summary>
    public static class OrcAutomationTestsModule
    {
        public static IServiceCollection AddOrcAutomationTests(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<ISetupAutomationService, SetupAutomationService>();

            return serviceCollection;
        }
    }
}
