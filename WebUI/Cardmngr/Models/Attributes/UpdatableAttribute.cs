namespace Cardmngr.Models.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class UpdatableAttribute : Attribute
{
    public UpdatableAttribute()
    {
    }
}
