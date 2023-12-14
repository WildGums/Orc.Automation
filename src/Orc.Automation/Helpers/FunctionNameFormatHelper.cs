namespace Orc.Automation;

using System.Linq;
using System.Text;
using Catel;

public static class FunctionNameFormatHelper
{
    public static string FormatMethodName(string methodName, params object[] args)
    {
        var stepNameBuilder = new StringBuilder(methodName.SplitCamelCase());

        if (args.Length > 0)
        {
            stepNameBuilder.Append(" (");
        }

        var argIndices = Enumerable.Range(0, args.Length)
            .Select(index => $"'{{{index}}}'");

        stepNameBuilder.AppendJoin(", ", argIndices);

        if (args.Length > 0)
        {
            stepNameBuilder.Append(")");
        }

        return stepNameBuilder.ToString();
    }
}
