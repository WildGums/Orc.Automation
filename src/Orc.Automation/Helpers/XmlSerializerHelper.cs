﻿namespace Orc.Automation
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using Catel.IoC;
    using Catel.Runtime.Serialization;

    public static class XmlSerializerHelper
    {
        private static readonly Dictionary<Type, ISerializationValueConverter> SerializationConverters = new();
        private static readonly Dictionary<Type, ISerializationValueConverter> DeserializationConverters = new();

        public static string SerializeValue(object value)
        {
            if (value is null)
            {
                return string.Empty;
            }

            var valueType = value.GetType();

            var converter = GetValueConverter(SerializationConverters, valueType);
            value = converter?.ConvertFrom(value) ?? value;

            var xmlSerializer = SerializationFactory.GetXmlSerializer();

            using var stream = new MemoryStream();
            xmlSerializer.Serialize(value, stream);
            var strResult = Encoding.Default.GetString(stream.ToArray());
            
            return strResult;
        }

        public static object? DeserializeValue(string text, Type type)
        {
            ArgumentNullException.ThrowIfNull(text);
            ArgumentNullException.ThrowIfNull(type);

            var converter = GetValueConverter(DeserializationConverters, type);

            type = converter?.ToType ?? type;

            var xmlSerializer = SerializationFactory.GetXmlSerializer();

#pragma warning disable IDISP001 // Dispose created.
            var stream = text.ToStream();
#pragma warning restore IDISP001 // Dispose created.

            var result = xmlSerializer.Deserialize(type, stream);

            result = converter?.ConvertTo(result) ?? result;

            return result;
        }

        private static ISerializationValueConverter GetValueConverter(IDictionary<Type, ISerializationValueConverter> converters, Type type)
        {
            if (converters.TryGetValue(type, out var converter))
            {
                return converter;
            }
            
            var converterType = type.FindGenericTypeImplementation<ISerializationValueConverter>(Assembly.GetExecutingAssembly())
                                ?? type.FindGenericTypeImplementation<ISerializationValueConverter>(type.Assembly);

            //TODO:Find a better way
            if (converterType is null)
            {
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    try
                    {
                        converterType = type.FindGenericTypeImplementation<ISerializationValueConverter>(assembly);
                        if (converterType is not null)
                        {
                            break;
                        }
                    }
                    catch
                    {
                        //Do nothing
                    }
                }
            }

            if (converterType is null)
            {
                return converter;
            }

#pragma warning disable IDISP004 // Don't ignore created IDisposable
            converter = converterType.GetTypeFactory().CreateInstanceWithParametersAndAutoCompletion(converterType) as ISerializationValueConverter;
#pragma warning restore IDISP004 // Don't ignore created IDisposable
            converters.Add(type, converter);

            return converter;
        }
    }
}
