///////////////////////////////////////
#region Namespace Directives

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RowYourBoat.Serialization;
using System;
using System.IO;
using System.Runtime.Serialization;

#endregion
///////////////////////////////////////

namespace RowYourBoat.Test.Serialization
{
    /// <summary>
    /// Unit tests addressing functionality within the "RowYourBoat.Serialization.JSONDataHandler" class.
    /// </summary>
    [TestClass]
    public class JSONDataHandler_spec
    {
        ////////////////////////////////////////
        #region Test File Path

        private string _testFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\JSONTestContent.xml";

        #endregion

        ////////////////////////////////////////
        #region Constructor

        /// <summary>
        /// Initializer for test conditions
        /// </summary>
        public JSONDataHandler_spec()
        {
            HomoSapien homoSapien = new HomoSapien { Age = 30, Name = "Charlie" };
            JSONDataHandler.WriteToFile(homoSapien, _testFilePath);
        }

        #endregion

        ////////////////////////////////////////
        #region TestContext Components (Auto-Generated)

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #endregion

        ////////////////////////////////////////
        #region Unit Tests (Methods)

        // public static object DeserializeFromReadStream(StreamReader _stream, Type _objectType);

        [TestMethod]
        public void DeserializeFromReadStream_EmptyString_Object()
        {
            object vehicle = new object();
            StreamReader _sReader = new StreamReader(_testFilePath);
            vehicle = JSONDataHandler.DeserializeFromReadStream(_sReader, typeof(object));
            Assert.IsNotNull(vehicle);
        }

        [TestMethod]
        public void DeserializeFromReadStream_Null_Object()
        {
            object vehicle = new object();
            vehicle = JSONDataHandler.DeserializeFromReadStream(null, typeof(object));
            Assert.IsNotNull(vehicle);
        }

        [TestMethod]
        public void DeserializeFromReadStream_WrongType_Object()
        {
            StreamReader _sReader = new StreamReader(_testFilePath);
            var vehicle = JSONDataHandler.DeserializeFromReadStream(_sReader, typeof(object));
            Assert.IsNotNull(vehicle);
        }

        [TestMethod]
        public void DeserializeFromReadStream_CorrectType_InitializedObject()
        {
            HomoSapien sapienOne = new HomoSapien { Age = 30, Name = "Charlie" };
            HomoSapien sapienTwo = new HomoSapien();
            StreamReader _sReader = new StreamReader(_testFilePath);
            sapienTwo = JSONDataHandler.DeserializeFromReadStream(_sReader, typeof(HomoSapien)) as HomoSapien;
            Assert.AreEqual(sapienOne.Age, sapienTwo.Age);
            Assert.AreEqual(sapienOne.Name, sapienTwo.Name);
        }

        // public static object ReadFromFile(Type _objectType, string _fullPath);

        [TestMethod]
        public void ReadFromFile_EmptyString_Object()
        {
            object vehicle = new object();
            vehicle = JSONDataHandler.ReadFromFile(Type.GetType("System.object"), String.Empty);
            Assert.IsNotNull(vehicle);
        }

        [TestMethod]
        public void ReadFromFile_Null_Object()
        {
            object vehicle = new object();
            vehicle = JSONDataHandler.ReadFromFile(null, String.Empty);
            Assert.IsNotNull(vehicle);
        }

        /// <remarks>
        /// Is null if casted directly as custom object type, must experiment with this to find a better way 
        /// of implementing the ReadFromFile method and its components.
        /// </remarks>
        [TestMethod]
        public void ReadFromFile_WrongType_Object()
        {
            var sapien = JSONDataHandler.ReadFromFile(Type.GetType("System.Int32"), _testFilePath);
            Assert.IsNotNull(sapien);
        }

        [TestMethod]
        public void ReadFromFile_CorrectType_InitializedObject()
        {
            HomoSapien sapienOne = new HomoSapien { Age = 30, Name = "Charlie" };
            HomoSapien sapienTwo = new HomoSapien();
            sapienTwo = JSONDataHandler.ReadFromFile(typeof(HomoSapien), _testFilePath) as HomoSapien;
            Assert.AreEqual(sapienOne.Age, sapienTwo.Age);
            Assert.AreEqual(sapienOne.Name, sapienTwo.Name);
        }

        // public static void SerializeToWriteStream(StreamWriter _stream, object _content);

        [ExpectedException(typeof(NullReferenceException))]
        [TestMethod]
        public void SerializeToWriteStream_NullStream_NullReferenceExceptionThrown()
        {
            JSONDataHandler.SerializeToWriteStream(null, new object());
        }

        [ExpectedException(typeof(NullReferenceException))]
        [TestMethod]
        public void SerializeToWriteStream_NullObject_NullReferenceExceptionThrown()
        {
            JSONDataHandler.SerializeToWriteStream(new StreamWriter("somepathname"), null);
        }

        // public static void WriteToFile(object _content, string _fullPath);

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void WriteToFile_NullPath_ArgumentNullExceptionThrown()
        {
            JSONDataHandler.WriteToFile(new object(), null);
        }

        [ExpectedException(typeof(DirectoryNotFoundException))]
        [TestMethod]
        public void WriteToFile_InvalidPath_DirectoryNotFoundExceptionThrown()
        {
            JSONDataHandler.WriteToFile(new object(), "Z:\\test.textnstuff");
        }

        [ExpectedException(typeof(NullReferenceException))]
        [TestMethod]
        public void WriteToFile_NullObject_NullReferenceExceptionThrown()
        {
            JSONDataHandler.WriteToFile(null, "somepathname");
        }

        #endregion

        ////////////////////////////////////////
        #region Child Classes (Used in Testing)

        [DataContract]
        public class HomoSapien
        {
            [DataMember]
            public string Name { get; set; }

            [DataMember]
            public int Age { get; set; }
        }

        #endregion
    }
}
