using Blazored.Modal;
using Blazored.Modal.Services;
using Cardmngr.Components.Modals;

namespace Cardmngr.Services;

public partial class TeamMemberSelectionDialog(IModalService modal)
{
    public async Task<Onlyoffice.Api.Models.IUser?> ShowAsync(ProjectModel project, ModalOptions options)
    {
        var parameters = new ModalParameters
        {
            { "Items", project.Team.ToList() },
            { "ItemRender", RenderUser }
        };

        var result = await modal.Show<SelectionModal<Onlyoffice.Api.Models.IUser>>("Выберите ответственного", parameters, options).Result;

        return result.Confirmed ? (Onlyoffice.Api.Models.IUser)result.Data! : null;
    }
}
