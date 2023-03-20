namespace Orc.Automation.Services;

using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Automation;

public class SetupAutomationService : ISetupAutomationService
{
#pragma warning disable IDISP006 // Implement IDisposable
    public AutomationSetup? CurrentSetup { get; private set; }
#pragma warning restore IDISP006 // Implement IDisposable

    public virtual AutomationSetup Setup(string executableFileLocation, Condition findMainWindowCondition, string? args = null)
    {
        var automationSetup = new AutomationSetup();

        var numWaits = 0;

        var process = args is null 
            ? Process.Start(executableFileLocation) 
            : Process.Start(executableFileLocation, args);

        do
        {
            ++numWaits;
            Thread.Sleep(10);
        }
        while (process is null && numWaits < 50);

        if (process is null)
        {
            throw new Exception($"Failed to find '{executableFileLocation}'");
        }
        automationSetup.Process = process;

        var desktop = AutomationElement.RootElement;
        if (desktop is null)
        {
            throw new Exception("Unable to get Desktop")
            {
                HelpLink = null,
                HResult = 0,
                Source = null
            };
        }
        automationSetup.Desktop = desktop;
        numWaits = 0;

        AutomationElement mainWindow = null;
        do
        {
            try
            {
                mainWindow = desktop.FindFirst(TreeScope.Children, findMainWindowCondition);
            }
            catch
            {
                //Do nothing
            }


            ++numWaits;
            Thread.Sleep(200);
        }
        while (mainWindow is null && numWaits < 500);

        if (mainWindow is null)
        {
            throw new Exception("Failed to find Main window");
        }

        automationSetup.MainWindow = mainWindow;

#pragma warning disable IDISP003 // Dispose previous before re-assigning
        CurrentSetup = automationSetup;
#pragma warning restore IDISP003 // Dispose previous before re-assigning

        return automationSetup;
    }
}