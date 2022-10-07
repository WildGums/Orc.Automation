namespace Orc.Automation.Converters
{
    using System.Windows;

    public class SerializableSize
    {
        public double Width { get; set; }
        public double Height { get; set; }
    }

    internal class SizeSerializationConverter : SerializationValueConverterBase<Size, SerializableSize>
    {
        public override object? ConvertFrom(Size value)
        {
            return new SerializableSize
            {
                Width = value.Width,
                Height = value.Height
            };
        }

        public override object? ConvertTo(SerializableSize value)
        {
            return new Size
            {
                Width = value.Width,
                Height = value.Height
            };
        }
    }
}
