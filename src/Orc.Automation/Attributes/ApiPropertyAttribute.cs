namespace Orc.Automation
{
    using System;

    public class AutomationAccessType : AutomationAttribute
    {

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
    }
}
