namespace Orc.Automation;

using System;
using System.Linq;
using Catel.Reflection;
using Controls;

public static class TestHostAutomationElementExtensions
{
    public static bool TryLoadControl(this TestHostAutomationControl testHost, Type controlType, out string testHostAutomationId, params string[] resources)
    {
        ArgumentNullException.ThrowIfNull(testHost);
        ArgumentNullException.ThrowIfNull(controlType);

        var controlAssembly = controlType.Assembly;

        var controlTypeFullName = controlType.GetSafeFullName();
        var controlAssemblyLocation = controlAssembly.Location;

        testHostAutomationId = string.Empty;

        if (!testHost.TryLoadAssembly(controlAssemblyLocation))
        {
            testHostAutomationId =  $"Error! Can't load control assembly from: {controlAssemblyLocation}";

            return false;
        }

        foreach (var resource in resources ?? Enumerable.Empty<string>())
        {
            if (!testHost.TryLoadResources(resource))
            {
                testHostAutomationId = $"Error! Can't load control resource: {resource}";
            }
        }

        var testedControlAutomationId = testHost.PutControl(controlTypeFullName);
        if (string.IsNullOrWhiteSpace(testedControlAutomationId) || testedControlAutomationId.StartsWith("Error"))
        {
            testHostAutomationId =  $"Error! Can't put control inside test host control: {controlTypeFullName}";

            return false;
        }

        testHostAutomationId = testedControlAutomationId;

        return true;
    }
}