using System.IO.Compression;

namespace PBIXUnpacker;
public class Unpacker
{
    public void UnpackPbix(string pbixRoute, string outRoute)
    {
        //Check if file exists
        //Check permissions on outRoute
        ZipFile.ExtractToDirectory(pbixRoute,outRoute);
        //OpenFile
            //unzip it
            //Prettyfy it    
    }
}
