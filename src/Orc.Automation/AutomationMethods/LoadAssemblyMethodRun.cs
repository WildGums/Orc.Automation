namespace Orc.Automation.AutomationMethods;

using System.Collections.Generic;
using System.Reflection;
using System;
using System.IO;
using System.Linq;
using System.Windows;

public class LoadAssemblyMethodRun : NamedAutomationMethodRun
{
    private readonly HashSet<string> _loadedAssemblyNames = new();

    public override bool TryInvoke(FrameworkElement owner, AutomationMethod method, out AutomationValue? result)
    {
        result = AutomationValue.FromValue(false);

        var location = method.Parameters.FirstOrDefault()?.ExtractValue() as string;
        if (string.IsNullOrWhiteSpace(location))
        {
            return false;
        }

        return LoadAssembly(location);
    }
    
    private bool LoadAssembly(string location)
    {
        try
        {
            var assembly = Assembly.LoadFile(location);

            LoadAssembly(assembly.GetName(), Path.GetDirectoryName(location));

            return assembly is not null;
        }
        catch (Exception ex)
        {
            Console.Write(ex);

            return false;
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                //TODO
                try
                {
                    if (!string.IsNullOrWhiteSpace(rootDirectory))
                    {
                        LoadAssembly(Path.Combine(rootDirectory, referencedAssembly.Name + ".dll"));
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
}
