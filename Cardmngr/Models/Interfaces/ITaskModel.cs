using Onlyoffice.Api.Common;
using Onlyoffice.Api.Models;

namespace Cardmngr.Models.Interfaces;

public interface ITaskModel : IModel, IWork
{
    bool CanCreateSubtask { get; }
    bool CanCreateTimeSpend { get; }
    bool CanReadFiles { get; }
    int? Progress { get; }
    TaskPriority Priority { get; set; }
    IUser? UpdatedBy { get; }
    IEnumerable<IUser> Responsibles { get; }
    IEnumerable<ISubtaskModel> Subtasks { get; }
    IMilestoneModel? Milestone { get; set; }
    IProjectModel Project { get; }
    IStatusColumnModel StatusColumn { get; set; }

    void AddSubtask(ISubtaskModel subtask);
    bool RemoveSubtask(ISubtaskModel subtask);
    void AddRangeResponsibles(params IUser[] responsibles);
    void ClearResponsibles();
}

public interface ISubtaskModel : IModel
{
    int TaskId { get; }
    Status Status { get; }
    IUser? Responsible { get; set; }

    void Close();
    void Open();
}
