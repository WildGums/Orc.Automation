namespace Orc.FilterBuilder.Automation
{
    using System.Globalization;
    using Catel.MVVM;

    internal static class DisplayNameConverterHelper
    {
        private static readonly ObjectToDisplayNameConverter DisplayNameConverter = new();

        public static string? ToDisplayString(this object? value)
        {
            if (value is null)
            {
                return (string?) DisplayNameConverter.Convert(null, null, null, CultureInfo.CurrentCulture);
            }

            if (value is string stringValue)
            {
                return stringValue;
            }

            return (string?) DisplayNameConverter.Convert(value, value.GetType(), null, CultureInfo.CurrentCulture);
        }
    }
}
