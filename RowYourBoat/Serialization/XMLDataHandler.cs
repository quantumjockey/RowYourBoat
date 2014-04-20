///////////////////////////////////////
#region Namespace Directives

using System;
using System.IO;
using System.Xml.Serialization;

#endregion
///////////////////////////////////////

namespace RowYourBoat.Serialization
{
    /// <summary>
    /// Operations for serializing XML data to and from various data streams.
    /// </summary>
    public static class XMLDataHandler
    {
        ////////////////////////////////////////
        #region File Read/Write

        /// <summary>
        /// Reads data from the specified path and serializes it into an object of the specified type.
        /// </summary>
        /// <param name="_objectType">The type to be serialized.</param>
        /// <param name="_fullPath">The full path from which data is being obtained.</param>
        /// <returns>Deserialized object data cast as System.Object.</returns>
        public static object ReadFromFile(Type _objectType, string _fullPath)
        {
            object _contentVehicle = new object();

            if (File.Exists(_fullPath))
            {
                StreamReader reader = new StreamReader(_fullPath);
                _contentVehicle = DeserializeFromReadStream(reader, _objectType);
            }

            return _contentVehicle;
        }

        /// <summary>
        /// Serializes object data and writes it to the specified path in XML format.
        /// </summary>
        /// <param name="_content">The object being serialized and written to file.</param>
        /// <param name="_fullPath">The full path to which data is being written.</param>
        public static void WriteToFile(object _content, string _fullPath)
        {
            if (_fullPath == null)
                throw new ArgumentNullException("Path cannot be null. Please specify a valid path.");
            if (_content == null)
                throw new NullReferenceException("Object being written to file cannot be null.");

            StreamWriter writer = new StreamWriter(_fullPath);
            SerializeToWriteStream(writer, _content);
        }

        #endregion

        ////////////////////////////////////////
        #region Stream Read/Write (Derived from TextWriter)

        /// <summary>
        /// Reads data from the specified stream and serializes it into an object of the specified type.
        /// </summary>
        /// <param name="_stream">The stream from which data is being obtained.</param>
        /// <param name="_objectType">The type to be serialized.</param>
        /// <returns>Deserialized object data cast as System.Object.</returns>
        public static object DeserializeFromReadStream(StreamReader _stream, Type _objectType)
        {
            object _content = new object();

            try
            {
                using (_stream)
                {
                    XmlSerializer dataSerializer = new XmlSerializer(_objectType);
                    _content = dataSerializer.Deserialize(_stream);
                    _stream.Close();
                }
            }
            catch
            {
                _content = Activator.CreateInstance(_objectType);
            }

            return _content;
        }

        /// <summary>
        /// Serializes object data and writes it to the specified stream in XML format.
        /// </summary>
        /// <param name="_stream">The stream to which data is being serialized and written.</param>
        /// <param name="_content">The object being serialized and written to the stream.</param>
        public static void SerializeToWriteStream(StreamWriter _stream, object _content)
        {
            if (_stream == null)
                throw new NullReferenceException("Stream cannot be null.");
            if (_content == null)
                throw new NullReferenceException("Object being serialized cannot be null.");

            using (_stream)
            {
                XmlSerializer dataSerializer = new XmlSerializer(_content.GetType());
                dataSerializer.Serialize(_stream, _content);
                _stream.Close();
            }
        }

        #endregion
    }
}
