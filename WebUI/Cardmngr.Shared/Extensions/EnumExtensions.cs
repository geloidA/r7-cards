using System.ComponentModel;

namespace Cardmngr.Shared.Extensions;

public static class EnumExtensions
{
    public static string GetDesc(this Enum value)
    {
        var fieldInfo = value.GetType().GetField(value.ToString());
        var descriptionAttribute = (DescriptionAttribute)fieldInfo!
            .GetCustomAttributes(typeof(DescriptionAttribute), false)
            .FirstOrDefault()!;

        return descriptionAttribute?.Description ?? value.ToString();
    }
}
