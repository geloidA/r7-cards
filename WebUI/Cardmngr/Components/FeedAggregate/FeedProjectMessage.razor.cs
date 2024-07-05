using Cardmngr.Domain.Entities;
using Cardmngr.Utils;
using KolBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Cardmngr.Components.FeedAggregate;

public partial class FeedProjectMessage : KolComponentBase
{
    private readonly Dictionary<string, MsgInfo> _msgTypes = new()
    {
        { "task", new MsgInfo(new Icons.Filled.Size20.LayerDiagonalAdd(), Color.Accent, "Добавлена задача") },
        { "participant", new MsgInfo(new Icons.Filled.Size20.PersonAdd(), Color.Success, "Добавлен(ы) участник(и)") },
        { "milestone", new MsgInfo(new MyIcons.Size20.Milestone(), Color.Neutral, "Добавлена веха") },
        { "project", new MsgInfo(new Icons.Filled.Size20.BookAdd(), Color.Warning, "Добавлен проект") }
    };

    [Parameter, EditorRequired]
    public Feed Feed { get; set; } = null!;

    private static MarkupString RenderHtml(string htmlString)
    {
        return new MarkupString(htmlString);
    }
}
