﻿using Cardmngr.Domain.Enums;

namespace Cardmngr.Extensions;

public static class DomainExtensions
{
    public static Onlyoffice.Api.Models.Common.Status ToStatus(this StatusType statusType) => (Onlyoffice.Api.Models.Common.Status)statusType;
}
