namespace Cardmngr.Utils;

public class HeaderProjectInfo
{
    private string? _title;
    public string? Title
    {
        get => _title;
        set
        {
            _title = value;
            TitleChanged?.Invoke();
        }
    }
    
    public Func<Task>? OpenInfoFunc { get; set; }

    public event Action? TitleChanged;
    public void Clean()
    {
        Title = null;
        OpenInfoFunc = null;
    }
}
