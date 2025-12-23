namespace Orc.Automation;

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using Catel.Logging;
using Microsoft.Extensions.Logging;
using Win32;

[TemplatePart(Name = "PART_HostGrid", Type = typeof(Grid))]
public partial class TestHost : Control
{
    private static readonly ILogger Logger = LogManager.GetLogger(typeof(TestHost));

    private readonly HashSet<string> _loadedAssemblyNames = new ();

    private Grid _hostGrid;

    public override void OnApplyTemplate()
    {
        _hostGrid = GetTemplateChild("PART_HostGrid") as Grid;
        if (_hostGrid is null)
        {
            throw Logger.LogErrorAndCreateException<InvalidOperationException>("Can't find template part 'PART_HostGrid'");
        }
    }

    public void ClearControls()
    {
        _hostGrid?.Children.Clear();
    }

    public string PutControl(string fullName)
    {
        if (_hostGrid is null)
        {
            return "Error: Host grid is null";
        }

        var controlType = TypeHelper.GetTypeByName(fullName);
        if (controlType is null)
        {
            return $"Error: Can't find control Type {fullName}";
        }

        if (Activator.CreateInstance(controlType) is not FrameworkElement control)
        {
            return $"Error: Can't instantiate control Type {controlType}";
        }

        var newAutomationId = Guid.NewGuid().ToString();

        control.SetCurrentValue(AutomationProperties.AutomationIdProperty, newAutomationId);

        _hostGrid.Children.Add(control);

        return newAutomationId;
    }

    public bool LoadResources(string resourcesPath)
    {
        try
        {
            var uri = new Uri(resourcesPath, UriKind.Absolute);
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = uri });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            return false;
        }
            
        return true;
    }

    public void LoadUnmanaged(string path)
    {
        Kernel32.LoadLibraryA(path);
    }

    public Assembly LoadAssembly(string assemblyPath)
    {
        try
        {
            var assembly = Assembly.LoadFile(assemblyPath);

            LoadAssembly(assembly.GetName(), Path.GetDirectoryName(assemblyPath));

            return assembly;
        }
        catch (Exception ex)
        {
            Console.Write(ex);

            return null;
        }
    }

    private void LoadAssembly(AssemblyName assemblyName, string? rootDirectory = null)
    {
        if (_loadedAssemblyNames.Contains(assemblyName.FullName))
        {
            return;
        }

        var loadedAssembly = AppDomain.CurrentDomain.Load(assemblyName);

        var referencedAssemblies = loadedAssembly.GetReferencedAssemblies();

        foreach (var referencedAssembly in referencedAssemblies)
        {
            try
            {
                LoadAssembly(referencedAssembly, rootDirectory);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);

                //TODO
                try
                {
                    if (!string.IsNullOrWhiteSpace(rootDirectory))
                    {
                        LoadAssembly(Path.Combine(rootDirectory, $"{referencedAssembly.Name}.dll"));
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);

                    return;
                }
            }

            _loadedAssemblyNames.Add(assemblyName.FullName);
        }
    }

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new TestHostAutomationPeer(this);
    }
}
