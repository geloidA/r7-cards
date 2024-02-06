using Bunit;
using Cardmngr.Components.Header;

namespace Cardmngr.Tests;

public class ColorThemeChangerTests : TestContext
{
    [Fact]
    public void Test()
    {
        var cut = RenderComponent<ColorThemeChanger>();
    }
}
