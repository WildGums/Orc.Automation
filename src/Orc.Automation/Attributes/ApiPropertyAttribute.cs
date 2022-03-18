namespace Orc.Automation
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class SerializationAutomationConverter : AutomationAttribute
    {
        public Type ConverterType { get; set; }
    }

    public class AutomationAccessType : AutomationAttribute
    {
        public Type DefaultOwnerType { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ApiPropertyAttribute : AutomationAttribute
    {
        public ApiPropertyAttribute()
        {
            
        }

        public ApiPropertyAttribute(string originalName)
        {
            OriginalName = originalName;
        }

        public string OriginalName { get; set; }
        public Type OwnerType { get; set; }
    }
}
