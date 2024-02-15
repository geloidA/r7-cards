using Cardmngr.Domain.Entities.Base;
using Cardmngr.Domain.Enums;

namespace Cardmngr.Domain.Entities
{
    public class OnlyofficeTaskStatus : EntityBase<int>
    {
        public StatusType StatusType { get; set; }
        public bool CanChangeAvailable { get; set; }
        public string Image { get; set; }
        public string ImageType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public int Order { get; set; }
        public bool IsDefault { get; set; }
        public bool Available { get; set; }
    }
}
