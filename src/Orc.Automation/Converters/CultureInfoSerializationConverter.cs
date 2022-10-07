﻿namespace Orc.Automation.Converters
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class SerializableCultureInfo
    {
        public string? CultureName { get; set; }
    }

    public class CultureInfoSerializationConverter : SerializationValueConverterBase<CultureInfo, SerializableCultureInfo>
    {
        private readonly Dictionary<string, CultureInfo> _availableCultures = CultureInfo.GetCultures(CultureTypes.AllCultures)
            .Where(culture => !string.IsNullOrEmpty(culture.Name) && !string.IsNullOrEmpty(culture.Parent.Name))
            .OrderBy(culture => culture.DisplayName).ToDictionary(x => x.Name, x => x);

        public override object? ConvertFrom(CultureInfo value)
        {
            return new SerializableCultureInfo { CultureName = value.Name };
        }

        public override object? ConvertTo(SerializableCultureInfo value)
        {
            return _availableCultures.TryGetValue(value.CultureName, out var cultureInfo) ? cultureInfo : null;
        }
    }

    public class SerializableCultureInfoList
    {
        public List<SerializableCultureInfo>? Cultures { get; set; }
    }

    public class CultureInfoListSerializationConverter : SerializationValueConverterBase<List<CultureInfo>, SerializableCultureInfoList>
    {
        private readonly CultureInfoSerializationConverter _cultureInfoConverter = new CultureInfoSerializationConverter();

        public override object ConvertFrom(List<CultureInfo> value)
        {
            return new SerializableCultureInfoList
            {
                Cultures = value?.Select(x => (SerializableCultureInfo)_cultureInfoConverter.ConvertFrom(x)).ToList()
            };
        }

        public override object ConvertTo(SerializableCultureInfoList value)
        {
            return value?.Cultures?.Select(x => (CultureInfo)_cultureInfoConverter.ConvertTo(x)).ToList();
        }
    }
}
