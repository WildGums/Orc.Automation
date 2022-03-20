namespace Orc.Automation
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class ActiveAutomationPropertyAttribute : AutomationAttribute
    {
        public ActiveAutomationPropertyAttribute()
        {
            
        }

        public ActiveAutomationPropertyAttribute(string originalName)
        {
            OriginalName = originalName;
        }

        public string OriginalName { get; set; }
        public Type OwnerType { get; set; }
    }
}
