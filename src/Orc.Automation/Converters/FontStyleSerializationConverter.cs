namespace Orc.Automation.Converters
{
    using System.Windows;

    public class SerializableFontStyle
    {
        public int FontStyleIndex { get; set; }
    }

    public class FontStyleSerializationConverter : SerializationValueConverterBase<FontStyle, SerializableFontStyle>
    {
        public override object? ConvertFrom(FontStyle value)
        {
            var index = 0;
            if (Equals(value, FontStyles.Normal))
            {
                index = 1;
            }

            if (Equals(value, FontStyles.Oblique))
            {
                index = 2;
            }
          
            return new SerializableFontStyle
            {
                FontStyleIndex = index
            };
        }

        public override object? ConvertTo(SerializableFontStyle value)
        {
            var index = value.FontStyleIndex;
            return index switch
            {
                0 => FontStyles.Italic,
                1 => FontStyles.Normal,
                2 => FontStyles.Oblique,

                _ => FontStyles.Normal
            };
        }
    }
}
