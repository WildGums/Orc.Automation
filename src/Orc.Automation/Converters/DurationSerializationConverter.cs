namespace Orc.Automation.Converters
{
    using System;
    using System.Windows;

    public class SerializableDuration
    {
        public TimeSpan Duration { get; set; }
    }

    public class DurationSerializationConverter : SerializationValueConverterBase<Duration, SerializableDuration>
    {
        public override object ConvertFrom(Duration value)
        {
            return new SerializableDuration
            {
                Duration = value.TimeSpan
            };
        }

        public override object ConvertTo(SerializableDuration value)
        {
            return new Duration(value.Duration);
        }
    }
}
