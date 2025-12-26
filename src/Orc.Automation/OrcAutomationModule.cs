namespace Orc.Automation
{
    using Catel.Services;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Orc.Automation.Services;

    /// <summary>
    /// Core module which allows the registration of default services in the service collection.
    /// </summary>
    public static class OrcAutomationModule
    {
        public static IServiceCollection AddOrcAutomation(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<ISetupAutomationService, SetupAutomationService>();
            serviceCollection.TryAddSingleton<IAutomationTestAccessService, AutomationTestAccessService>();

            serviceCollection.AddSingleton<ILanguageSource>(new LanguageResourceSource("Orc.Automation", "Orc.Automation.Properties", "Resources"));

            return serviceCollection;
        }
    }
}
