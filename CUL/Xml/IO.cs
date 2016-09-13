using System.IO;
using System.Xml.Serialization;

namespace Clapton.Xml
{
    public static class IO
    {
        /// <summary>
        /// Constructs an XML Serializable object from a stored file. 
        /// </summary>
        /// <typeparam name="T">XML serializable class type</typeparam>
        /// <param name="filePath">FilePath leading to the XML file to be read.</param>
        /// <returns>Object of <see cref="T"/> constructed from the XML file.</returns>
		public static T ReadXml<T>(string filePath) where T : new()
		{
			if (filePath == null || !File.Exists(filePath))
			{
				throw new FileNotFoundException("File not found.", filePath);
			}

			FileStream stream = null;
			T result = new T();
            var serializer = new XmlSerializer(typeof(T));

            try
			{
				stream = new FileStream(filePath, FileMode.Open);
                result = (T)serializer.Deserialize(stream);
			}
			finally
			{
				if (stream != null)
				{
					stream.Close();
					stream.Dispose();
				}
			}

			return result;
		}

        /// <summary>
        /// Writes an XML serializable object to disk.
        /// </summary>
        /// <typeparam name="T">XML Serialaizable class type.</typeparam>
        /// <param name="saveData">Object of XML Serializable class type.</param>
        /// <param name="savePath">Path of file to write.</param>
        public static void WriteXml<T>(this T saveData, string savePath) where T : new()
        {
            var dir = Path.GetDirectoryName(Path.GetFullPath(savePath)) ?? "";
            Directory.CreateDirectory(dir);

            FileStream stream = null;
            var serializer = new XmlSerializer(typeof(T));

            try
            {
                stream = new FileStream(savePath, FileMode.Create);
                serializer.Serialize(stream, saveData);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
            }
        }
    }
}
