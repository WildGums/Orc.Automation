namespace Orc.Automation.Converters;

using System.Windows;

public class SerializableFontStretch
{
    public int Stretch { get; set; }
}

public class FontStretchSerializationConverter : SerializationValueConverterBase<FontStretch, SerializableFontStretch>
{
    public override object? ConvertFrom(FontStretch value)
    {
        return new SerializableFontStretch
        {
            Stretch = value.ToOpenTypeStretch()
        };
    }

    public override object? ConvertTo(SerializableFontStretch value)
    {
        return FontStretch.FromOpenTypeStretch(value.Stretch);
    }
}