namespace Orc.Automation.Converters;

using System;
using System.Windows.Media;
using Catel.Logging;
using Microsoft.Extensions.Logging;

public class SerializableColor
{
    public string? Color { get; set; }
}

public class ColorSerializationConverter : SerializationValueConverterBase<Color, SerializableColor>
{
    private static readonly ILogger Logger = LogManager.GetLogger(typeof(ColorSerializationConverter));

    private const char ArgbSeparator = ';';

    public override object? ConvertFrom(Color value)
    {
        return new SerializableColor
        {
            Color = $"{value.A}{ArgbSeparator}{value.R}{ArgbSeparator}{value.G}{ArgbSeparator}{value.B}"
        };
    }

    public override object? ConvertTo(SerializableColor value)
    {
        var color = value.Color;

        try
        {
            if (!string.IsNullOrEmpty(color))
            {
                var colorParts = color.Split(ArgbSeparator);

                return Color.FromArgb(System.Convert.ToByte(colorParts[0]), System.Convert.ToByte(colorParts[1]),
                    System.Convert.ToByte(colorParts[2]), System.Convert.ToByte(colorParts[3]));
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Cannot modify '{0}' to a color", color);
        }

        return default(Color);
    }
}
