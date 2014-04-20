///////////////////////////////////////
#region Namespace Directives

using System;
using System.IO;
using System.Runtime.Serialization.Json;

#endregion
///////////////////////////////////////

namespace RowYourBoat.Serialization
{
    /// <summary>
    /// Operations for serializing JSON data to and from various data streams.
    /// </summary>
    /// <remarks>
    /// In order to use these methods to serialize data to and from objects as JSON, those objects must be implemented
    /// with service-based data contract attributes. Reference material for the process of/syntax for implementing these 
    /// can be found on < http://msdn.microsoft.com/en-us/library/ms733127(v=vs.110).aspx >
    /// Components of this source have been referenced directly from:
    /// < http://msdn.microsoft.com/en-us/library/bb412179(v=vs.110).aspx > as accessed on 4/20/2014.
    /// </remarks>
    public static class JSONDataHandler
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
        /// Serializes object data and writes it to the specified path in JSON format.
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
            object content = Activator.CreateInstance(_objectType);
            string contentVehicle;

            try
            {
                using (_stream)
                {
                    // retrieve serialized object data from the stream as a string
                    contentVehicle = _stream.ReadToEnd();

                    // create a byte array and store the string-converted stream to it
                    byte[] dataVehicle = new byte[contentVehicle.Length * sizeof(char)];
                    Buffer.BlockCopy(contentVehicle.ToCharArray(), 0, dataVehicle, 0, dataVehicle.Length);

                    // create a memory stream from the byte array and move the cursor to its zero position
                    MemoryStream _memStream = new MemoryStream(dataVehicle);
                    _memStream.Position = 0;

                    // deserialize the JSON-fromatted object data to an object
                    DataContractJsonSerializer dataSerializer = new DataContractJsonSerializer(_objectType);
                    content = dataSerializer.ReadObject(_memStream);
                    _stream.Close();
                }
            }
            catch
            {
                content = Activator.CreateInstance(_objectType);
            }

            return content;
        }

        /// <summary>
        /// Serializes object data and writes it to the specified stream in JSON format.
        /// </summary>
        /// <param name="_stream">The stream to which data is being serialized and written.</param>
        /// <param name="_content">The object being serialized and written to the stream.</param>
        public static void SerializeToWriteStream(StreamWriter _stream, object _content)
        {
            string _contentVehicle;

            // create a memory stream and serialize JSON data to it
            MemoryStream _memStream = new MemoryStream();
            DataContractJsonSerializer dataSerializer = new DataContractJsonSerializer(_content.GetType());
            dataSerializer.WriteObject(_memStream, _content);

            // read JSON-serialized object data from the memory stream and convert it to a string
            _memStream.Position = 0;
            StreamReader reader= new StreamReader(_memStream);
            _contentVehicle = reader.ReadToEnd();
            reader.Close();

            // write the string containing JSON-serialized object data to the stream
            _stream.Write(_contentVehicle);
            _stream.Close();
        }

        #endregion
    }
}
