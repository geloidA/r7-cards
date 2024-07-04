using Cardmngr.Shared.Utils;
using FluentAssertions;

namespace Cardmngr.Shared.Tests;

public class BindingAddressTests
{
    [Theory]
    [InlineData("http://localhost:5000", "127.0.0.1", 5000, "http")]
    [InlineData("https://localhost:5001", "127.0.0.1", 5001, "https")]
    [InlineData("https://+:5000", "0.0.0.0", 5000, "https")]
    [InlineData("http://*:5000", "0.0.0.0", 5000, "http")]
    public void Parse_ReturnsCorrectAddress(string address, string host, int port, string scheme)
    {
        var bindingAddress = BindingAddress.Parse(address);

        bindingAddress.Host.Should().Be(host);
        bindingAddress.Port.Should().Be(port);
        bindingAddress.Scheme.Should().Be(scheme);
    }
}
