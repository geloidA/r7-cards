using Microsoft.AspNetCore.Components;

namespace Cardmngr;

public class HeaderTitle
{
    private RenderFragment? content;
    public RenderFragment? Content
    {
        get => content;
        set
        {
            content = value;
            ContentChanged?.Invoke();
        }
    }
    

    public event Action? ContentChanged;
}
