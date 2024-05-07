using System.ComponentModel;

namespace Cardmngr.Domain.Enums
{
    public enum Status
    {
        [Description("Не принята")]
        NotAccept,
        [Description("Открыта")]
        Open,
        [Description("Закрыта")]
        Closed,
        [Description("Отложена")]
        Disable,
        [Description("Неклассифицировано")]
        Unclassified,
        [Description("Не в вехе")]
        NotInMilistone
    }
}
