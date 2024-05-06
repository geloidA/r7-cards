using Cardmngr.Domain.Entities;
using Cardmngr.Report;
using Cardmngr.Reports;
using Microsoft.FluentUI.AspNetCore.Components;
using Onlyoffice.Api;
using Onlyoffice.Api.Common;

namespace Cardmngr.Pages.ReportsTabs;

public partial class EfficiencyFactorTab
{
    private bool generating;
    private readonly EfficiencyFactorReportRequest reportRequest = new();
    private List<Group> groups = [];
    private List<Project> projectsData = [];
    private List<UserInfo> usersData = [];


    protected override async Task OnInitializedAsync()
    {
        groups = await GroupClient.GetGroupsAsync().ToListAsync();
        projectsData = await ProjectClient.GetProjectsAsync().ToListAsync();
        usersData = await PeopleClient.GetUsersAsync()
            .Cast<UserInfo>()
            .ToListAsync();
    }

    #region Search
    private void OnProjectSearch(OptionsSearchEventArgs<Project> e)
    {
        e.Items = projectsData.Where(x => x.Title.Contains(e.Text, StringComparison.InvariantCultureIgnoreCase));
    }

    private void OnUserSearch(OptionsSearchEventArgs<UserInfo> e)
    {
        e.Items = usersData.Where(x => x.DisplayName.Contains(e. Text, StringComparison.InvariantCultureIgnoreCase));
    }

    private void OnGroupSearch(OptionsSearchEventArgs<Group> e)
    {
        e.Items = groups.Where(x => x.Name.Contains(e.Text, StringComparison.InvariantCultureIgnoreCase));
    }
    #endregion

    private async Task GenerateReport()
    {
        generating = true;

        var scope = ServiceProvider.CreateScope(); // for escape downloading saveFile.js when sidebar is opening
        var reportService = scope.ServiceProvider.GetRequiredService<IReportService>();

        var tasks = await GetFilteredTasksAsync();

        if (tasks.Count == 0)
        {
            generating = false;
            ToastService.ShowError("Данные не найдены");
            return;
        }
        
        reportService.Generator = new EfficiencyFactorReport 
        { 
            Tasks = tasks,
            Statuses = await ProjectClient.GetTaskStatusesAsync().ToListAsync()
        };

        await reportService.GenerateReport($"Коэффициент эффективности-{DateTime.Now:dd.MM.yyyy HH_mm}");

        generating = false;
    }

    private ValueTask<List<OnlyofficeTask>> GetFilteredTasksAsync()
    {
        return ProjectClient.GetFilteredTasksAsync(GetFilterBuilder())
            .Where(x => reportRequest.Group.Any() || 
                !reportRequest.Responsibles.Any() || 
                reportRequest.Responsibles.Any(r => x.Responsibles.Any(tR => tR.Id == r.Id)))

            .Where(x => !reportRequest.Projects.Any() || 
                reportRequest.Projects.Any(p => p.Id == x.ProjectOwner.Id))

            .ToListAsync();
    }

    private FilterBuilder GetFilterBuilder()
    {
        var filter = FilterTasksBuilder.Instance
            .DeadlineStart(reportRequest.Deadline.Start)
            .DeadlineStop(reportRequest.Deadline.End);

        if (reportRequest.Group.Any())
        {
            Console.WriteLine("Group: " + reportRequest.Group.Single().Name);
            filter = filter.Department(reportRequest.Group.Single().Id);
        }

        return filter;
    }
}
