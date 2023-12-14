namespace Orc.Automation.Tests;

using MethodBoundaryAspect.Fody.Attributes;
using ScenarioManagement;

public class SuspendScenarioRecordingAttribute : OnMethodBoundaryAspect
{
    public override void OnEntry(MethodExecutionArgs arg)
    {
        ScenarioManager.IsRecordingStepsSuspended = true;
    }

    public override void OnExit(MethodExecutionArgs arg)
    {
        ScenarioManager.IsRecordingStepsSuspended = false;
    }
}