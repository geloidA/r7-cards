using Cardmngr.Models.Attributes;
using Onlyoffice.Api.Models;

namespace Cardmngr.Models;

public abstract class ModelBase
{
    [Updatable]
    public DateTime Created { get; protected set; }

    [Updatable]
    public IUser? CreatedBy { get; protected set; }

    [Updatable]
    public DateTime Updated { get; protected set;}

    /// <summary>
    /// Update properties that marked with <see cref="UpdatableAttribute"/>
    /// </summary>
    /// <remarks>
    /// Not invoke <see cref="ModelChanged"/>
    /// </remarks>
    /// <param name="source">Source model</param>
    /// <exception cref="ArgumentException">Invalid TApiModel type</exception>
    protected void UpdateProperties<TApiModel>(TApiModel source)
    {
        ArgumentNullException.ThrowIfNull(source);

        var selfType = GetType();
        var sourceType = source.GetType();

        var properties = selfType.GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(UpdatableAttribute)));

        foreach (var selfProp in properties)
        {
            var sourceProp = sourceType.GetProperty(selfProp.Name) ?? throw new ArgumentException("TApiModel type not contains property " + selfProp.Name);

            if (sourceProp.CanRead && selfProp.CanWrite)
            {
                selfProp.SetValue(this, sourceProp.GetValue(source));
            }
        }
    }
}
