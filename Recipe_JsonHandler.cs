namespace RecipeRack;

/// <summary>
/// This class handles writing the raw text info to a json file for the user. 
/// </summary>
internal class Recipe_JsonHandler
{
    /// <summary>
    /// Write a Json File to the directory for the tips document.
    /// </summary>
    public static void WriteToJsonFile_Tips(string filePath, string RichTextToWrite, bool append = false)
    {
        TextWriter writer = null;
        try
        {
            string contentsToWriteToFile = 
                JsonConvert.SerializeObject(RichTextToWrite, Formatting.Indented);

            writer = new StreamWriter(filePath, append);
            writer.Write(contentsToWriteToFile);
        }
        finally
        {
            writer?.Close();
        }
    }

    /// <summary>
    /// Read from a Json File in the directory for the tips document.
    /// </summary>
    public static string ReadFromJsonFile_Tips(string filePath)
    {
        TextReader reader = null;

        try
        {
            reader = new StreamReader(filePath);
            string fileContents = reader.ReadToEnd();

            return (string)JsonConvert.DeserializeObject(fileContents);
        }
        finally
        {
            reader?.Close();
        }
    }
}
