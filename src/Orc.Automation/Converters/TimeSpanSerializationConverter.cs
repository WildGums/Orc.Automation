namespace Orc.Automation.Converters
{
    using System;

    public class SerializableTimeSpan
    {
        public double TotalMilliseconds { get; set; }
    }

    public class TimeSpanSerializationConverter : SerializationValueConverterBase<TimeSpan, SerializableTimeSpan>
    {
        public override object? ConvertFrom(TimeSpan value)
        {
            return new SerializableTimeSpan
            {
                TotalMilliseconds = value.TotalMilliseconds
            };
        }

        public override object? ConvertTo(SerializableTimeSpan value)
        {
            return TimeSpan.FromMilliseconds(value.TotalMilliseconds);
        }
    }
}
