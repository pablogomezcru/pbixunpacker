using Xunit;
using PBIXUnpacker;

namespace UnpackerTests;

public class UnitTest
{

    public Unpacker _Unpacker { get; set; }

    public UnitTest()
    {
        _Unpacker = new();
    }
    [Fact]
    public void Test1()
    {
        _Unpacker.UnpackPbix("nothing");
        Assert.Equal(1, 1);
    }
}