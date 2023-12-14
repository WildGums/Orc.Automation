namespace Orc.Automation.Converters;

using System;

public class SerializableUri
{
    public string? UriString { get; set; }
    public UriKind Kind { get; set; }
}

public class UriSerializationConverter : SerializationValueConverterBase<Uri, SerializableUri>
{
    public override object? ConvertFrom(Uri value)
    {
        return new SerializableUri
        {
            Kind = value.IsAbsoluteUri ? UriKind.Absolute : UriKind.Relative,
            UriString = value.ToString(),
        };
    }

    public override object? ConvertTo(SerializableUri value)
    {
        if (string.IsNullOrEmpty(value.UriString))
        {
            return null;
        }

        return new Uri(value.UriString, value.Kind);
    }
}