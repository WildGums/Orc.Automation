namespace Orc.Automation.Recroding.Models
{
    using System.Collections.Generic;

    public class EventListenerConfiguration
    {
        public EventListenerConfiguration()
        {
            InputEventsNames = new List<string>();
        }

        public List<string> InputEventsNames { get; set; }
    }
}
