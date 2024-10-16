﻿namespace Orc.Automation.Tests;

using System.Windows.Automation;
using Controls;

public abstract class AppUiTestsBase<TMainWindow> : UiTestsBase
    where TMainWindow : class
{
    protected abstract string AppName { get; }

    protected virtual string NetVersion => "net8.0-windows";
    protected virtual string Company => "Simply Effective Solutions";
    protected virtual string Channel => "alpha";
    protected abstract string RelativeProjectPath { get; }
    protected abstract string RelativeAppConfigurationFolderPath { get; }

    protected virtual string ShellWindowId => "ShellWindowId";
    protected virtual string TestHostId => "TestHostId";
    protected string AbsoluteProjectPath { get; private set; }
    protected string AbsoluteAppConfigurationFolderPathProjectPath { get; private set; }

    protected override string Args => "-p";
    protected override string ExecutablePath => TestPathHelper.GetExecutablePath(AppName, NetVersion);

    protected override Condition FindMainWindowCondition =>
        new PropertyCondition(AutomationElement.AutomationIdProperty, ShellWindowId);

    protected TemporaryFileSystemContext TempTestFileSystemContext { get; private set; }

    protected TMainWindow MainWindow
    {
        get
        {
            if (Setup is null)
            {
                throw new("Can't setup test");
            }

            if (Setup.MainWindow is null)
            {
                throw new("Can't find main window");
            }

            return Setup.MainWindow.As<TMainWindow>();
        }
    }

    public override void SetUp()
    {
        var moveDirectory = TestPathHelper.GetTempDirectoryAbsolutePath(AppName, "MoveFolder");

        TempTestFileSystemContext = new(moveDirectory);

        TempTestFileSystemContext.CreateDirectory(moveDirectory, FileSystemContextEntryAction.Delete);

        AbsoluteProjectPath = TempTestFileSystemContext.Copy(
            TestPathHelper.GetTestDirectoryAbsolutePath(RelativeProjectPath),
            TestPathHelper.GetTempDirectoryAbsolutePath(AppName, GetType().Name + "_Project"),
            FileSystemContextEntryAction.Delete);

        AbsoluteAppConfigurationFolderPathProjectPath = TempTestFileSystemContext.Copy(
            TestPathHelper.GetTestDirectoryAbsolutePath(RelativeAppConfigurationFolderPath),
            GetConfigurationFolderPath(), FileSystemContextEntryAction.Move);

        base.SetUp();

        LoadAssembly(TestPathHelper.GetAutomationTestAssemblyPath());
        LoadAssembly(TestPathHelper.GetAppTestAssemblyPath(AppName));
    }

    protected virtual string GetConfigurationFolderPath() => TestPathHelper.GetConfigurationFolderPath(AppName, Company);

    public override void TearDown()
    {
        try
        {
            base.TearDown();

            Wait.UntilInputProcessed(1000);

            TempTestFileSystemContext.Dispose();
        }
        catch
        {
            // ignored
        }
        finally
        {
            Wait.UntilInputProcessed(1000);
        }
    }

    protected void LoadAssembly(string assemblyPath)
    {
        var testHost = Setup?.MainWindow?.Find<TestHostAutomationControl>(TestHostId, numberOfWaits: 10);
        testHost?.TryLoadAssembly(assemblyPath);
    }
}
