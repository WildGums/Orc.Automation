namespace Orc.Automation;

using System.Linq;
using Catel;

public static class FunctionNameFormatHelper
{
    public static string FormatMethodName(string methodName, params object[] args)
    {
        var stepNameFormat = methodName.SplitCamelCase();

        if (args.Length > 0)
        {
            stepNameFormat += " (";
        }

        stepNameFormat += string.Join(",", Enumerable.Range(0, args.Length).Select(index => $"'{{{index}}}'"));

        if (args.Length > 0)
        {
            stepNameFormat += ")";
        }

        return stepNameFormat;
    }
}
