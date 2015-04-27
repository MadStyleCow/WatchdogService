using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace WatchdogService.Utilities
{
    public static class XMLSerializer
    {
        public static string SerializeToString(this object pObjectInstance)
        {
            var serializer = new XmlSerializer(pObjectInstance.GetType());
            var sb = new StringBuilder();

            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, pObjectInstance);
            }

            return sb.ToString();
        }

        public static T DeserializeFromString<T>(string pObjectData)
        {
            return (T)DeserializeFromString(pObjectData, typeof(T));
        }

        public static object DeserializeFromString(string pObjectData, Type pType)
        {
            var serializer = new XmlSerializer(pType);
            object result;

            using (TextReader reader = new StringReader(pObjectData))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }

        public static object DeserializeFromFile(string pFile, Type pType)
        {
            if (!File.Exists(pFile))
            {
                throw new FileNotFoundException();
            }

            var serializer = new XmlSerializer(pType);
            object result;

            using (var reader = new StreamReader(pFile))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }

        public static object DeserializeFromStream(Stream pStream, Type pType)
        {
            var serializer = new XmlSerializer(pType);
            object result;

            using (pStream)
            {
                result = serializer.Deserialize(pStream);
            }

            return result;
        }
    }
}
