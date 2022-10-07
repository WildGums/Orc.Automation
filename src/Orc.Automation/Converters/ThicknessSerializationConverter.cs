namespace Orc.Automation.Converters
{
    using System.Windows;

    public class SerializableThickness
    {
        public double Left { get; set; }
        public double Top { get; set; }
        public double Right { get; set; }
        public double Bottom { get; set; }
    }

    internal class ThicknessSerializationConverter : SerializationValueConverterBase<Thickness, SerializableThickness>
    {
        public override object? ConvertFrom(Thickness value)
        {
            return new SerializableThickness
            {
                Left = value.Left,
                Top = value.Top,
                Right = value.Right,
                Bottom = value.Bottom,
            };
        }

        public override object? ConvertTo(SerializableThickness value)
        {
            return new Thickness
            {
                Left = value.Left,
                Top = value.Top,
                Right = value.Right,
                Bottom = value.Bottom,
            };
        }
    }
}
