using Blazored.Modal;
using Blazored.Modal.Services;
using Cardmngr.Components.Modals;
using Cardmngr.Domain.Entities;

namespace Cardmngr.Services;

public partial class TeamMemberSelectionDialog(IModalService modal)
{
    public async Task<UserInfo?> ShowAsync(IEnumerable<UserInfo> team, ModalOptions options)
    {
        var parameters = new ModalParameters
        {
            { "Items", team.ToList() },
            { "ItemRender", RenderUser }
        };

        var result = await modal.Show<SelectionModal<UserInfo>>("Выберите ответственного", parameters, options).Result;

        return result.Confirmed ? (UserInfo)result.Data! : null;
    }
}
