using System;
using System.IO;
using System.Xml.Serialization;

namespace Technical
{
    /// <summary>
    /// Serialize helper.
    /// </summary>
    public static class SerializeHelper
    {

        /// <summary>
        /// Deserialize file.
        /// </summary>
        /// <typeparam name="T">Return object type.</typeparam>
        /// <param name="file">File to deserialize.</param>
        /// <returns>Deserialized object.</returns>
        public static T DeserializeFile<T>(string file)
        {
            T result;
            try
            {
                var serialize = new XmlSerializer(typeof (T));
                var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                result = (T)serialize.Deserialize(fileStream);
                fileStream.Close();
            }
            catch
            {
                result = default(T);
            }
            return result;
        }

        /// <summary>
        /// Serialize an object to file.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="obj">Object to serialize.</param>
        /// <param name="file">File.</param>
        public static void SerializeFile<T>(T obj, string file)
        {
            try
            {
                var writer = new XmlSerializer(typeof(T));
                using (var stream = new FileStream(file, FileMode.OpenOrCreate))
                {
                    writer.Serialize(stream, obj);
                    stream.Close();
                }
            }
            catch
            {
            }
        }

    }
}
