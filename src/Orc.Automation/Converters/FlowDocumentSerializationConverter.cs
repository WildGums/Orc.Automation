namespace Orc.Automation.Converters
{
    using System.IO;
    using System.Windows.Documents;
    using System.Windows.Markup;
    using System.Xml;

    public class SerializableFlowDocument
    {
        public string Contents { get; set; }
    }

    public class FlowDocumentSerializationConverter : SerializationValueConverterBase<FlowDocument, SerializableFlowDocument>
    {
        public override object ConvertFrom(FlowDocument doc)
        {
            var contents = XamlWriter.Save(doc);

            return new SerializableFlowDocument { Contents = contents };
        }

        public override object ConvertTo(SerializableFlowDocument value)
        {
            using var stringReader = new StringReader(value.Contents);
            using var xmlTextReader = new XmlTextReader(stringReader);
            return (FlowDocument)XamlReader.Load(xmlTextReader);
        }
    }
}
