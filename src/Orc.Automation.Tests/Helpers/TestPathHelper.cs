namespace Orc.Automation.Tests;

using System;
using System.IO;
using NUnit.Framework;

public static class TestPathHelper
{
    public static string GetExecutablePath(string appName, string netVersion = "net8.0-windows") =>
        @$"{TestContext.CurrentContext.TestDirectory}\..\..\{appName}\{netVersion}\{appName}.exe";

    public static string GetConfigurationFolderPath(string appName, string company = "Simply Effective Solutions",
        string channel = "alpha")
    {
        var configurationFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        configurationFolderPath = $"{configurationFolderPath}\\{company}\\{appName}";
        if (!string.IsNullOrWhiteSpace(channel))
        {
            configurationFolderPath += $"_{channel}";
        }

        return configurationFolderPath;
    }

    public static string GetAutomationTestAssemblyPath()
        => GetTestDirectoryAbsolutePath("Orc.Automation.Tests.dll");

    public static string GetAppTestAssemblyPath(string appName) 
        => GetTestDirectoryAbsolutePath($"{appName}.Tests.dll");

    public static string GetTestDirectoryAbsolutePath(string relativePath = "") =>
        Path.Combine(TestContext.CurrentContext.TestDirectory, relativePath);

    public static string GetTempDirectoryAbsolutePath(string appName, string relativePath = "") =>
        Path.Combine(Path.GetTempPath(), appName, relativePath);
}
