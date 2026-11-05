using Xunit;
using Xunit.Abstractions;

namespace backend.Tests;

public class HashTest
{
    private readonly ITestOutputHelper _output;

    public HashTest(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void TestBcryptHash()
    {
        string hash = BCrypt.Net.BCrypt.HashPassword("admin123", 11);
        _output.WriteLine("BCRYPT_HASH=" + hash);
        Assert.True(BCrypt.Net.BCrypt.Verify("admin123", hash));
    }
}
