namespace Cardmngr.Utils;

public class HeaderInteractionService
{
    private string? _title;
    public string? ProjectTitle
    {
        get => _title;
        set
        {
            _title = value;
            ProjectTitleChanged?.Invoke();
        }
    }
    
    public Func<Task>? OpenProjectInfoFunc { get; set; }

    public event Action? ProjectTitleChanged;
    
    public void CleanProjectTitle()
    {
        ProjectTitle = null;
        OpenProjectInfoFunc = null;
    }

    private bool _headerCollapsed;
    public bool HeaderCollapsed
    {
        get => _headerCollapsed;
        set
        {
            if (value != _headerCollapsed)
            {
                _headerCollapsed = value;
                HeaderCollapsedChanged?.Invoke();
            }
        }
    }

    public event Action? HeaderCollapsedChanged;
}
