using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Xml;
using Newtonsoft.Json;

namespace PBIXUnpacker;

public interface IUnpacker
{
    void PackPbix(string folderRoute, string pbixRoute);
    void UnpackPbix(string pbixRoute);
    void UnpackPbix(string pbixRoute, string outRoute);
}

public class Unpacker : IUnpacker
{
    public void UnpackPbix(string pbixRoute)
    {
        pbixRoute = pbixRoute ?? "";
        if (string.IsNullOrEmpty(pbixRoute)) throw new ArgumentException("pbixRoute cannot be null.");
        UnpackPbix(pbixRoute, Path.GetDirectoryName(pbixRoute) ?? "");
    }
    public void UnpackPbix(string pbixRoute, string outRoute)
    {
        pbixRoute = pbixRoute ?? "";
        outRoute = outRoute ?? "";
        // string filename = Path.GetFileName(pbixRoute);
        if (!File.Exists(pbixRoute)) throw new FileNotFoundException($"Route \"{pbixRoute}\" was not found");
        if (Path.GetExtension(pbixRoute) != ".pbix") throw new FileNotFoundException($"File \"{pbixRoute}\" is not .pbix");
        string temproute = $"{Path.GetDirectoryName(pbixRoute)}\\tmp";
        ZipFile.ExtractToDirectory(pbixRoute, temproute, overwriteFiles: true);
        IndentFiles(temproute);
        //Check permissions on outRoute
        //OpenFile
        //unzip it
        //Prettyfy it    
        //   }  else throw new ArgumentException("pbixRoute cannot be null.");
    }
    private void IndentFiles(string temproute)
    {
        if (!Directory.Exists(temproute)) throw new InvalidOperationException("Tried to indent files in nonexistent directory.");
        string[] files = Directory.GetFiles(temproute);
        foreach (string file in files)
        {
            var fileString = File.ReadAllText(file, Encoding.Unicode);
            if (isJson(fileString)) IndentJsonFile(file);
            else if (isXML(fileString)) IndentXMLFile(file);
        }
        string[] directories = Directory.GetDirectories(temproute);
        foreach (string dir in directories)
        {
            IndentFiles(dir);
        }
    }
    private bool isXML(string file)
    {
        XmlDocument doc = new XmlDocument();
        try { doc.LoadXml(file); }
        catch { return false; }
        return true;
    }
    private bool isJson(string file)
    {
        try { var tmpObj = JsonDocument.Parse(file); }
        catch { return false; }
        return true;
    }
    private void IndentXMLFile(string fileRoute)
    {
        var xmlString = File.ReadAllText(fileRoute, Encoding.Unicode);
        XmlWriter writer = null!;
        try
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);

            // Create an XmlWriterSettings object with the correct options.
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = ("\t");

            // Create the XmlWriter object and write some content.
            writer = XmlWriter.Create(fileRoute, settings);
            doc.Save(writer);

            writer.Flush();
        }
        finally
        {
            if (writer != null)
                writer.Close();
        }
    }
    private void IndentJsonFile(string fileRoute)
    {
        var jsonString = File.ReadAllText(fileRoute, Encoding.Unicode);
        Newtonsoft.Json.Linq.JObject t = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(jsonString);

        var x = JsonConvert.SerializeObject(t, Newtonsoft.Json.Formatting.Indented);
        File.WriteAllText(fileRoute, x);
    }

    public void PackPbix(string folderRoute, string pbixRoute)
    {
        try
        {
            ZipFile.CreateFromDirectory(folderRoute, pbixRoute, CompressionLevel.Fastest, true);
        }
        catch (Exception e)
        {
            var foo = e;
        }
    }
}


