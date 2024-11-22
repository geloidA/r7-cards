﻿using Blazored.Modal;
using Microsoft.AspNetCore.Components;

namespace Cardmngr.Components.Modals.ConfirmModals;

public abstract partial class ConfirmModalBase : ComponentBase
{
    [CascadingParameter] protected BlazoredModalInstance Modal { get; set; } = default!;
    [Parameter] public string OkTitle { get; set; } = "ОК";
    [Parameter] public string CancelTitle { get; set; } = "Отмена";
    [Parameter] public string HeaderText { get; set; } = "Вы уверены?";

    [Parameter] public string? AdditionalText { get; set; }

    [Parameter] public string? OkBgColor { get; set; }

    [Parameter] public string? OkTextColor { get; set; }
}
