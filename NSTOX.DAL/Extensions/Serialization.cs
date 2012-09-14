using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace NSTOX.DAL.Extensions
{
    public static class SerializationHelper
    {
        public static string ToXML(this object obj)
        {
            StringBuilder sb = new StringBuilder();

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = false;
            settings.NewLineHandling = NewLineHandling.None;

            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(writer, obj);
            }

            return sb.ToString();
        }

        public static T Deserialize<T>(this byte[] content) where T : class
        {
            using (MemoryStream stream = new MemoryStream(content))
            {
                XmlSerializer deSerializer = new XmlSerializer(typeof(T));

                object o = deSerializer.Deserialize(stream);
                if (o != null)
                    return o as T;
            }
            return null;
        }

        public static T Deserialize<T>(this string filePath) where T : class
        {
            using (FileStream fStream = File.OpenRead(filePath))
            {
                XmlSerializer deSerializer = new XmlSerializer(typeof(T));

                object o = deSerializer.Deserialize(fStream);
                if (o != null)
                    return o as T;
            }
            return null;
        }
    }
}
