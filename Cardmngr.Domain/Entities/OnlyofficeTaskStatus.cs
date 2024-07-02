using Cardmngr.Domain.Entities.Base;
using Cardmngr.Domain.Enums;

namespace Cardmngr.Domain.Entities
{
    public record OnlyofficeTaskStatus : EntityBase<int>
    {
        public StatusType StatusType { get; init; }
        public bool CanChangeAvailable { get; init; }
        public string Image { get; init; }
        public string ImageType { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public string Color { get; init; }
        public int Order { get; init; }
        public bool IsDefault { get; init; }
        public bool Available { get; init; }
    }
}
