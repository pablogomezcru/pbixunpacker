using PBIXUnpacker;
// See https://aka.ms/new-console-template for more information
string pbixRoute = @"..\..\samples\pbix\sample.pbix";
string outRoute = @"..\..\samples\pbix\";
Unpacker u = new();
u.UnpackPbix(pbixRoute: pbixRoute, outRoute: outRoute);
Console.WriteLine("END");
