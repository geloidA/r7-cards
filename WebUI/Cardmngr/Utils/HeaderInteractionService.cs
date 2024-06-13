using Cardmngr.Domain.Entities;

namespace Cardmngr.Utils;

public class HeaderInteractionService
{
    private Project? _selectedProject;
    public Project? SelectedProject
    {
        get => _selectedProject;
        set
        {
            _selectedProject = value;
            SelectedProjectChanged?.Invoke();
        }
    }
    
    public Func<Task>? OpenProjectInfoFunc { get; set; }

    public event Action? SelectedProjectChanged;
    
    public void CleanSelectedProject()
    {
        SelectedProject = null;
        OpenProjectInfoFunc = null;
    }

    private bool _headerCollapsed = true;
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
