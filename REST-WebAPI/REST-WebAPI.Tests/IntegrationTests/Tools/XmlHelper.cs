using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace REST_WebAPI.Tests.IntegrationTests.Tools {
    public static class XmlHelper {

        public static StringContent SerializeToXml<T>(T obj) {
            var xmlSerializer = new XmlSerializer(typeof(T));
            var ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);

            using var stringWriter = new Utf8StringWriter();
            xmlSerializer.Serialize(stringWriter, obj, ns);

            return new StringContent(stringWriter.ToString(), Encoding.UTF8, "application/xml");
        }

        public static async Task<T?> ReadFromXmlAsync<T>(HttpResponseMessage response) {
            var xmlSerializer = new XmlSerializer(typeof(T));
            await using var stream = await response.Content.ReadAsStreamAsync();

            return (T?)xmlSerializer.Deserialize(stream);
        }

        private class Utf8StringWriter : StringWriter {
            public override Encoding Encoding => Encoding.UTF8;
        }
    }
}
