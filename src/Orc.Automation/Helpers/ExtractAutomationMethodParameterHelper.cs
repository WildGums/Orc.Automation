namespace Orc.Automation
{
    internal static class ExtractAutomationMethodParameterHelper
    {
        public static TValue? Extract<TValue>(this object[] parameters, int index)
        {
            return parameters[index] is TValue ? (TValue)parameters[index] : default;
        }
    }
}
