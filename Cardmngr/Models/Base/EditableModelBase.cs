using Cardmngr.Models.Attributes;
using Cardmngr.Models.Interfaces;

namespace Cardmngr.Models.Base;

public abstract class EditableModelBase<TApiModel> : ModelBase, IEditableModel<TApiModel>
{
    public abstract IEditableModel<TApiModel> EditableModel { get; }

    public virtual void Update(TApiModel source)
    {
        ArgumentNullException.ThrowIfNull(source);
        
        UpdateProperties(source);
        
        OnModelChanged();
    }

    /// <summary>
    /// Update properties that marked with <see cref="UpdatableAttribute"/>
    /// </summary>
    /// <remarks>
    /// Not invoke <see cref="ModelChanged"/>
    /// </remarks>
    /// <param name="source">Source model</param>
    /// <exception cref="ArgumentException">Invalid TApiModel type</exception>
    private void UpdateProperties(object source)
    {
        var selfType = GetType();
        var sourceType = source!.GetType();

        var properties = selfType.GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(UpdatableAttribute)));

        foreach (var selfProp in properties)
        {
            var sourceProp = sourceType.GetProperty(selfProp.Name) 
                ?? throw new ArgumentException($"Source's type - {sourceType.Name} not contain property - {selfProp.Name}");

            if (sourceProp.CanRead && selfProp.CanWrite)
            {
                selfProp.SetValue(this, sourceProp.GetValue(source));
            }
        }
    }
}