using Cardmngr.Domain.Entities;
using Cardmngr.Extensions;
using Cardmngr.Report;
using Cardmngr.Reports;
using Microsoft.FluentUI.AspNetCore.Components;
using Onlyoffice.Api;
using Onlyoffice.Api.Common;

namespace Cardmngr.Pages.ReportsTabs;

public partial class ProjectTasksTab
{
    private readonly ProjectTaskReportRequest reportRequest = new();
    private List<Project> projectsData = [];
    private List<UserInfo> usersData = [];
    private readonly IEnumerable<TaskStatusType> statusTypes = Enum.GetValues(typeof(TaskStatusType))
        .Cast<TaskStatusType>();    

    protected async override Task OnInitializedAsync()
    {
        projectsData = await ProjectClient.GetProjectsAsync().ToListAsync();
        usersData = await PeopleClient.GetUsersAsync()
            .Cast<UserInfo>()
            .ToListAsync();
    }

    private void OnProjectSearch(OptionsSearchEventArgs<Project> e)
    {
        e.Items = projectsData.Where(x => x.Title.Contains(e.Text, StringComparison.InvariantCultureIgnoreCase));
    }

    private void OnUserSearch(OptionsSearchEventArgs<UserInfo> e)
    {
        e.Items = usersData.Where(x => x.DisplayName.Contains(e. Text, StringComparison.InvariantCultureIgnoreCase));
    }

    private bool generating;

    private async Task GenerateReport()
    {
        generating = true;

        var scope = ServiceProvider.CreateScope(); // for escape downloading saveFile.js when sidebar is opening
        var reportService = scope.ServiceProvider.GetRequiredService<IReportService>();        

        var tasks = await GetFilteredTasksAsync().ToListAsync();

        if (tasks.Count == 0)
        {
            ToastService.ShowError("Данные не найдены");
            generating = false;
            return;
        }

        reportService.Generator = new TaskReportGenerator 
        {
            Tasks = tasks,
            Statuses = await TaskStatusClient.GetAllAsync().ToListAsync()
        };

        await reportService.GenerateReport($"Задачи_проектов {DateTime.Now:dd.MM.yyyy HH_mm}");
        await reportService.DisposeAsync();

        generating = false;
    }

    private async IAsyncEnumerable<OnlyofficeTask> GetFilteredTasksAsync()
    {
        await foreach (var task in FilterTasksByUserRequest(TaskClient.GetEntitiesAsync(CreateFilter())))
            yield return task;
    }

    private IAsyncEnumerable<OnlyofficeTask> FilterTasksByUserRequest(IAsyncEnumerable<OnlyofficeTask> tasks)
    {
        return tasks
            .Where(x => !reportRequest.Responsibles.Any() || reportRequest.Responsibles.Any(r => x.Responsibles.Any(tR => tR.Id == r.Id)))
            .Where(x => !reportRequest.Projects.Any() || reportRequest.Projects.Any(p => p.Id == x.ProjectOwner.Id))
            .Where(x => !reportRequest.Creators.Any() || reportRequest.Creators.Any(c => c.Id == x.CreatedBy.Id))
            .Where(InStartDateRange);
    }

    private FilterBuilder CreateFilter()
    {
        var filter = TaskFilterBuilder.Instance.MyProjects(true);

        if (reportRequest.OnlyDeadline)
        {
            filter = filter.DeadlineOutside();
        }
        else
        {
            filter = filter
                .DeadlineStart(reportRequest.DeadlineRange.Start)
                .DeadlineStop(reportRequest.DeadlineRange.End);
            
            if (reportRequest.TaskStatusType != TaskStatusType.None)
            {
                filter = filter.Status(reportRequest.TaskStatusType.ToDomainStatus());
            }
        }

        return filter;
    }

    private bool InStartDateRange(OnlyofficeTask task)
    {
        var startDate = task.StartDate?.Date;
        return startDate is null ||
            !reportRequest.StartDateRange.Start.HasValue || reportRequest.StartDateRange.Start?.Date <= startDate &&
                !reportRequest.StartDateRange.End.HasValue || reportRequest.StartDateRange.End?.Date >= startDate;
    }
}
