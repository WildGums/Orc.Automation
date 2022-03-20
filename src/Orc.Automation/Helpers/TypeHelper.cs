namespace Orc.Automation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class TypeHelper
    {
        public static Type GetTypeByName(string fullName)
        {
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
                    //var assembly2 = Assembly.LoadFile(@"C:\Source\Orc.Controls\output\Debug\Orc.Controls.Tests\net6.0-windows\ControlzEx.dll");
                    //AppDomain.CurrentDomain.Load(assembly2.GetName());

                    //var types = assembly.GetTypes();
                    //var type = types.FirstOrDefault(x => string.Equals(x.FullName, fullName));

                    continue;
                }
            }

            return null;
        }

        private static IList<Type> GetLoadableTypes(Assembly assembly)
        {
            if (assembly is null) throw new ArgumentNullException(nameof(assembly));
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
}
