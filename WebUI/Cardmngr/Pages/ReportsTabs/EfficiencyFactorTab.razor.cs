using Cardmngr.Domain.Entities;
using Cardmngr.Report;
using Cardmngr.Reports;
using Cardmngr.Shared.Extensions;
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
    private List<UserInfo> groupUsers = [];


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
            Users = reportRequest.Group.Any() ? groupUsers : reportRequest.Responsibles
        };

        await reportService.GenerateReport($"Эффективность пользователей-{DateTime.Now:dd.MM.yyyy HH_mm}");

        generating = false;
    }

    private async ValueTask<List<OnlyofficeTask>> GetFilteredTasksAsync()
    {
        var tasks = ProjectClient.GetFilteredTasksAsync(GetFilterBuilder());

        if (reportRequest.Group.Any())
        {
            groupUsers = await PeopleClient.GetFilteredUsersAsync(PeopleFilterBuilder.Instance
                .GroupId(reportRequest.Group.Single().Id).Simple())
                    .Cast<UserInfo>()
                    .ToListAsync();

            tasks = tasks.Where(x => x.Responsibles.Any(tR => groupUsers.Any(u => u.Id == tR.Id)));
        }
        else
        {
            tasks = tasks.Where(x => !reportRequest.Responsibles.Any() || 
                x.Responsibles.Any(tR => reportRequest.Responsibles.Any(r => r.Id == tR.Id)));
        }

        return await tasks
            .Where(x => !reportRequest.Projects.Any() || 
                reportRequest.Projects.Any(p => p.Id == x.ProjectOwner.Id))
            .Where(x => x.IsDeadlineOut() || x.Status == Domain.Enums.Status.Closed)
            .ToListAsync();
    }

    private TaskFilterBuilder GetFilterBuilder()
    {
        return TaskFilterBuilder.Instance
            .DeadlineStart(reportRequest.Deadline.Start)
            .DeadlineStop(reportRequest.Deadline.End);
    }
}
