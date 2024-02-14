using Cardmngr.Domain.Entities.Base;

namespace Cardmngr.Domain.Entities
{
    public class UserInfo : EntityBase<string>
    {
        public string DisplayName { get; set; }
        public string AvatarSmall { get; set; }
        public string ProfileUrl { get; set; }
    }
}
