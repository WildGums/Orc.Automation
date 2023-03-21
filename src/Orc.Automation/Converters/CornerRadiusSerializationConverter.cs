namespace Orc.Automation.Converters;

using System.Windows;

public class SerializableCornerRadius
{
    public double TopLeft { get; set; }
    public double BottomLeft { get; set; }
    public double TopRight { get; set; }
    public double BottomRight { get; set; }
}

public class CornerRadiusSerializationConverter : SerializationValueConverterBase<CornerRadius, SerializableCornerRadius>
{
    public override object? ConvertFrom(CornerRadius value)
    {
        return new SerializableCornerRadius
        {
            TopLeft = value.TopLeft,
            BottomLeft = value.BottomLeft,
            TopRight = value.TopRight,
            BottomRight = value.BottomRight,
        };
    }

    public override object? ConvertTo(SerializableCornerRadius value)
    {
        return new CornerRadius
        {
            TopLeft = value.TopLeft,
            BottomLeft = value.BottomLeft,
            TopRight = value.TopRight,
            BottomRight = value.BottomRight,
        };
    }
}