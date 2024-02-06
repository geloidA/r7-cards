using Onlyoffice.Api.Models;
using OTask = Onlyoffice.Api.Models.Task;
using OTaskStatus = Onlyoffice.Api.Models.TaskStatus;

namespace Cardmngr.Tests.Models;

public static class ModelCreator
{
    public static ProjectModel GetModel()
    {
        var project = CreateProject();
        List<OTask> tasks = CreateTasks();
        List<OTaskStatus> statuses = CreateTaskStatuses();
        List<Milestone> milestones = CreateMilestones();
        IEnumerable<IUser> team = CreateTeam();

        return new ProjectModel(project, tasks, statuses, milestones, team);
    }

    public static MilestoneModel GetMilestone()
    {
        return new MilestoneModel(CreateMilestone(), ProjectModel.Empty);
    }

    private static Project CreateProject()
    {
        return new Project
        {
            Id = 1,
            Title = "Test Project",
            Description = "This is a test project",
            Status = 1,
            Responsible = CreateResponsible(),
            CanEdit = true,
            IsPrivate = false,
            Updated = DateTime.Now,
            CreatedBy = CreateCreatedBy(),
            Created = DateTime.Now,
            TaskCount = 2,
            TaskCountTotal = 2
        };
    }

    private static List<OTask> CreateTasks()
    {
        var task1 = new OTask
        {
            CanEdit = true,
            CanCreateSubtask = true,
            CanCreateTimeSpend = true,
            CanDelete = true,
            CanReadFiles = true,
            Id = 1,
            Title = "OTask 1",
            Description = "Description for OTask 1",
            Priority = 1,
            MilestoneId = 1,
            Milestone = CreateMilestone(),
            ProjectOwner = CreateProjectOwner(),
            Subtasks = [],
            Status = 1,
            Progress = 50,
            UpdatedBy = CreateUpdatedBy(),
            Created = DateTime.Now,
            CreatedBy = CreateCreatedBy(),
            Updated = DateTime.Now,
            Responsibles = [CreateResponsible()],
            CustomTaskStatus = 1,
            Deadline = DateTime.Now.AddDays(7),
            StartDate = DateTime.Now.AddDays(-1)
        };

        var task2 = new OTask
        {
            CanEdit = true,
            CanCreateSubtask = true,
            CanCreateTimeSpend = true,
            CanDelete = true,
            CanReadFiles = true,
            Id = 2,
            Title = "OTask 2",
            Description = "Description for OTask 2",
            Priority = 2,
            MilestoneId = 1,
            Milestone = CreateMilestone(),
            ProjectOwner = CreateProjectOwner(),
            Subtasks = [],
            Status = 1,
            Progress = 25,
            UpdatedBy = CreateUpdatedBy(),
            Created = DateTime.Now,
            CreatedBy = CreateCreatedBy(),
            Updated = DateTime.Now,
            Responsibles = [CreateResponsible()],
            CustomTaskStatus = 2,
            Deadline = DateTime.Now.AddDays(14),
            StartDate = DateTime.Now
        };

        return [task1, task2];
    }

    private static List<OTaskStatus> CreateTaskStatuses()
    {
        var status1 = new OTaskStatus
        {
            StatusType = 1,
            CanChangeAvailable = true,
            Id = 1,
            Image = "image1.png",
            ImageType = "png",
            Title = "Status 1",
            Description = "Description for Status 1",
            Color = "#FF0000",
            Order = 1,
            IsDefault = true,
            Available = true
        };

        var status2 = new OTaskStatus
        {
            StatusType = 2,
            CanChangeAvailable = true,
            Id = 2,
            Image = "image2.png",
            ImageType = "png",
            Title = "Status 2",
            Description = "Description for Status 2",
            Color = "#00FF00",
            Order = 2,
            IsDefault = false,
            Available = true
        };

        return [status1, status2];
    }

    private static List<Milestone> CreateMilestones()
    {
        var milestone1 = new Milestone
        {
            CanEdit = true,
            CanDelete = true,
            Id = 1,
            Title = "Milestone 1",
            Description = "Description for Milestone 1",
            ProjectOwner = CreateProjectOwner(),
            Deadline = DateTime.Now.AddDays(30),
            IsKey = true,
            IsNotify = false,
            ActiveTaskCount = 5,
            ClosedTaskCount = 3,
            Status = 1,
            Responsible = CreateResponsible(),
            Created = DateTime.Now,
            CreatedBy = CreateCreatedBy(),
            Updated = DateTime.Now
        };

        return [milestone1];
    }

    private static IEnumerable<IUser> CreateTeam()
    {
        var user1 = new UserProfile
        {
            Id = "1",
            UserName = "User1",
            IsVisitor = false,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Status = 1,
            ActivationStatus = 1,
            Terminated = null,
            Department = "IT",
            WorkFrom = DateTime.Now.AddDays(-30),
            DisplayName = "John D.",
            AvatarMedium = "avatar1_medium.png",
            Avatar = "avatar1.png",
            IsAdmin = true,
            IsLDAP = false,
            IsOwner = true,
            IsSSO = false,
            AvatarSmall = "avatar1_small.png",
            QuotaLimit = 100,
            UsedSpace = 50,
            DocsSpace = 20,
            MailSpace = 10,
            TalkSpace = 20,
            ProfileUrl = "https://example.com/users/1"
        };

        var user2 = new UserProfile
        {
            Id = "2",
            UserName = "User2",
            IsVisitor = false,
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@example.com",
            Status = 1,
            ActivationStatus = 1,
            Terminated = null,
            Department = "Marketing",
            WorkFrom = DateTime.Now.AddDays(-15),
            DisplayName = "Jane S.",
            AvatarMedium = "avatar2_medium.png",
            Avatar = "avatar2.png",
            IsAdmin = false,
            IsLDAP = true,
            IsOwner = false,
            IsSSO = true,
            AvatarSmall = "avatar2_small.png",
            QuotaLimit = 150,
            UsedSpace = 75,
            DocsSpace = 30,
            MailSpace = 15,
            TalkSpace = 30,
            ProfileUrl = "https://example.com/users/2"
        };

        return [user1, user2];
    }

    private static Responsible CreateResponsible()
    {
        return new Responsible
        {
            Id = "1",
            DisplayName = "Responsible User",
            AvatarSmall = "avatar_responsible_small.png",
            ProfileUrl = "https://example.com/responsible"
        };
    }

    private static ProjectOwner CreateProjectOwner()
    {
        return new ProjectOwner
        {
            Id = 1,
            Title = "Project Owner",
            Status = 1,
            IsPrivate = false
        };
    }

    private static UpdatedBy CreateUpdatedBy()
    {
        return new UpdatedBy
        {
            Id = "1",
            DisplayName = "Updated By User",
            AvatarSmall = "avatar_updatedby_small.png",
            ProfileUrl = "https://example.com/updatedby"
        };
    }

    private static CreatedBy CreateCreatedBy()
    {
        return new CreatedBy
        {
            Id = "1",
            DisplayName = "Created By User",
            AvatarSmall = "avatar_createdby_small.png",
            ProfileUrl = "https://example.com/createdby"
        };
    }

    private static Milestone CreateMilestone()
    {
        return new Milestone
        {
            CanEdit = true,
            CanDelete = true,
            Id = 1,
            Title = "Test Milestone",
            Description = "This is a test milestone",
            ProjectOwner = CreateProjectOwner(),
            Deadline = DateTime.Now.AddDays(15),
            IsKey = false,
            IsNotify = true,
            ActiveTaskCount = 2,
            ClosedTaskCount = 1,
            Status = 1,
            Responsible = CreateResponsible(),
            Created = DateTime.Now,
            CreatedBy = CreateCreatedBy(),
            Updated = DateTime.Now
        };
    }
}
