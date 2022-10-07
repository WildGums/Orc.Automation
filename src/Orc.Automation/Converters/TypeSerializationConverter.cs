namespace Orc.Automation.Converters
{
    using System;
    using Catel.Reflection;

    public class SerializableType
    {
        public string FullName { get; set; }
    }

    public class TypeSerializationConverter : SerializationValueConverterBase<Type, SerializableType>
    {
        public override object? ConvertFrom(Type value)
        {
            return new SerializableType
            {
                FullName = value?.GetSafeFullName()
            };
        }

        public override object? ConvertTo(SerializableType value)
        {
            var fullName = value?.FullName;
            if (string.IsNullOrWhiteSpace(fullName))
            {
                return null;
            }

            return Orc.Automation.TypeHelper.GetTypeByName(fullName);
        }
    }
}
