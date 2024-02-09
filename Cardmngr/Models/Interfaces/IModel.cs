using Onlyoffice.Api.Models;

namespace Cardmngr.Models.Interfaces;

public interface IModel
{
    DateTime Created { get; }
    IUser? CreatedBy { get; }
    DateTime Updated { get; }
    bool CanEdit { get; }
    bool CanDelete { get; }
    int Id { get; }
    string? Title { get; set; }
    string? Description { get; }

    event Action ModelChanged;
}

public interface IEditableModel<TApiModel> : IModel
{
    /// <summary>
    /// Updates self state by properties marked by <see cref="Attributes.UpdatableAttribute"/> 
    /// Clean up EditableModel after use
    /// </summary>
    void Update(TApiModel source);
    IEditableModel<TApiModel> EditableModel { get; }
}
