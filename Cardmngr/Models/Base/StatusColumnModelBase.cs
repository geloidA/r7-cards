using Cardmngr.Models.Interfaces;
using Onlyoffice.Api.Common;
using MyTaskStatus = Onlyoffice.Api.Models.TaskStatus;

namespace Cardmngr.Models.Base;

public abstract class StatusColumnModelBase : ObservableCollectionModel<ITaskModel>, IStatusColumnModel
{
    public StatusColumnModelBase(MyTaskStatus taskStatus, IProjectModel projectModel)
    {
        Project = projectModel;
        
        Status = (Status)taskStatus.StatusType;
        CanChangeAvailable = taskStatus.CanChangeAvailable;
        Id = taskStatus.Id;
        Image = taskStatus.Image;
        ImageType = taskStatus.ImageType;
        Title = taskStatus.Title ?? taskStatus.Id.ToString();
        Description = taskStatus.Description;
        Color = taskStatus.Color;
        Order = taskStatus.Order;
        IsDefault = taskStatus.IsDefault;
        Available = taskStatus.Available;
    }

    public Status Status { get; }

    public bool CanChangeAvailable { get; }

    public string? Image { get; }

    public string? ImageType { get; }

    public string? Color { get; }

    public int Order { get; }

    public bool IsDefault { get; }

    public bool Available { get; }

    public IProjectModel Project { get; }
}
