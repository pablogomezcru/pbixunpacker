using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Xml;
using Newtonsoft.Json;

namespace PBIXUnpacker;
public class Unpacker
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
        if (!File.Exists(pbixRoute)) throw new FileNotFoundException($"{pbixRoute} not found");
        if (Path.GetExtension(pbixRoute) != ".pbix") throw new FileNotFoundException($"{pbixRoute} is not .pbix");
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
            if (isJson(file)) IndentJsonFile(file);
            else if (isXML(file)) IndentXMLFile(file);
        }
        string[] directories = Directory.GetDirectories(temproute);
        foreach( string dir in directories)
        {
            IndentFiles(dir);
        }
    }

    private bool isXML(string file)
    {
        return _XMLFormattedFiles.Contains(Path.GetFileName(file));
    }

    private bool isJson(string file)
    {
        return _JsonFormattedFiles.Contains(Path.GetFileName(file));
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
        var t = JsonConvert.DeserializeObject(jsonString);
        var x = JsonConvert.SerializeObject(t, Newtonsoft.Json.Formatting.Indented);
        File.WriteAllText(fileRoute, x);
    }

    public void PackPbix(string folderRoute, string pbixRoute)
    {
        try
        {
            ZipFile.CreateFromDirectory(folderRoute, pbixRoute,CompressionLevel.Fastest, true);
        }
        catch (Exception e)
        {
            var foo = e;
        }
    }

    private static readonly string[] _JsonFormattedFiles = new string[] {
      "DiagramLayout",
      "Metadata",
      "Settings",
      "Layout"
  };

    private static readonly string[] _XMLFormattedFiles = new string[] {
     "[Content_Types].xml"
  };
}


