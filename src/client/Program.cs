using PBIXUnpacker;
// See https://aka.ms/new-console-template for more information
string pbixRoute = @"..\..\test\Versioned PBIX\sample.pbix";
Unpacker u = new();
u.UnpackPbix(pbixRoute: pbixRoute);
// u.PackPbix(folderRoute: unpackedRoute, pbixRoute: rezipRoute);
Console.WriteLine("END");
