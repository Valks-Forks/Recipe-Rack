using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Windows.Controls;

namespace Recipe_Rack
{

    
    class Recipe_JsonHandler
    {


        /// <summary>
        /// Writes the given object instance to a Json file.
        /// <para>Object type must have a parameterless constructor.</para>
        /// <para>Only Public properties and variables will be written to the file. These can be any type though, even other classes.</para>
        /// <para>If there are public properties/variables that you do not want written to the file, decorate them with the [JsonIgnore] attribute.</para>
        /// </summary>
        /// <typeparam name="T">The type of object being written to the file.</typeparam>
        /// <param name="filePath">The file path to write the object instance to.</param>
        /// <param name="objectToWrite">The object instance to write to the file.</param>
        /// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
        public static void WriteToJsonFile(string filePath, Recipes objectToWrite, bool append = false) 
        {
            TextWriter writer = null;
            try
            {
                var contentsToWriteToFile = JsonConvert.SerializeObject(objectToWrite, Formatting.Indented);
                writer = new StreamWriter(filePath, append);
                writer.Write(contentsToWriteToFile);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        /// <summary>
        /// Reads an object instance from an Json file.
        /// <para>Object type must have a parameterless constructor.</para>
        /// </summary>
        /// <typeparam name="T">The type of object to read from the file.</typeparam>
        /// <param name="filePath">The file path to read the object instance from.</param>
        /// <returns>Returns a new instance of the object read from the Json file.</returns>
        public static object ReadFromJsonFile<T>(string filePath) 
        {
            TextReader reader = null;
            try
            {
                reader = new StreamReader(filePath);
                var fileContents = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(fileContents);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        // --------------------------------------------------------------------------------------------------------------
        // This adds another section for handling the Tips Document 
        // Rather than try to edit established code and break everything, Im just going to add a similiar method just for the tips menu
        // This was the last part of project
        // 

        public static void WriteToJsonFile_Tips(string filePath, string RichTextToWrite, bool append = false)
        {
            TextWriter writer = null;
            try
            {
                var contentsToWriteToFile = JsonConvert.SerializeObject(RichTextToWrite, Formatting.Indented);
                writer = new StreamWriter(filePath, append);
                writer.Write(contentsToWriteToFile);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        public static string ReadFromJsonFile_Tips(string filePath)
        {
            TextReader reader = null;
            try
            {
                reader = new StreamReader(filePath);
                var fileContents = reader.ReadToEnd();
                return (string)JsonConvert.DeserializeObject(fileContents);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

    }
}