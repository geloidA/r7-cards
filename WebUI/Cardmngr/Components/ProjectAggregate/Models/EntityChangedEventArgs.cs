namespace Cardmngr.Components.ProjectAggregate.Models;

public class EntityChangedEventArgs<T>(EntityActionType actionType, T entity) : EventArgs
{
    public EntityActionType ActionType { get; set; } = actionType;
    public T Entity { get; set; } = entity;
}