namespace Orc.Automation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class TypeHelper
{
    public static Type GetTypeByName(string fullName)
    {
        ArgumentNullException.ThrowIfNull(fullName);

        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies)
        {
            try
            {
                var types = GetLoadableTypes(assembly);
                var type = types.FirstOrDefault(x => string.Equals(x.FullName, fullName));

                if (type is not null)
                {
                    return type;
                }
            }
            catch
            {
                //Do nothing
            }
        }

        return null;
    }

    private static IList<Type> GetLoadableTypes(Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);
            
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException e)
        {
            return (IList<Type>)e.Types?.Where(t => t is not null).ToList() ?? Array.Empty<Type>();
        }
    }
}