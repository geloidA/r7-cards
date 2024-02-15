using System.ComponentModel.DataAnnotations;
using Cardmngr.Models.Attributes;
using Cardmngr.Models.Interfaces;
using Onlyoffice.Api.Models;

namespace Cardmngr.Models.Base;

public abstract class ModelBase : IModel
{
    [Updatable]
    public DateTime Created { get; protected set; }

    [Updatable]
    public IUser? CreatedBy { get; protected set; }

    [Updatable]
    public DateTime Updated { get; protected set;}

    [Updatable]
    public bool CanEdit { get; protected set; }

    [Updatable]
    public bool CanDelete { get; protected set; }

    [Updatable]
    public int Id { get; protected set; }

    [Updatable]
    [Required(ErrorMessage = "Название обязательно для заполнения")]
    public string? Title { get; set; }

    [Updatable]
    public string? Description { get; set; }

    public event Action? ModelChanged;

    protected void OnModelChanged() => ModelChanged?.Invoke();
}
