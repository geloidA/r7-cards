using Cardmngr.Utils;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Cardmngr.Components.FeedAggregate;

public static class FeedMsgTypes
{
    public static readonly FeedMsgType Task = new(
        "task", 
        "Задачи", 
        new Icons.Filled.Size20.LayerDiagonalAdd(), 
        Color.Accent, 
        "Добавлена задача",
        "Добавление задач");

    public static readonly FeedMsgType Participant = new(
        "participant", 
        "Участники", 
        new Icons.Filled.Size20.PersonAdd(), 
        Color.Success, 
        "Добавлен(ы) участник(и)",
        "Добавление участников");

    public static readonly FeedMsgType Milestone = new(
        "milestone",
        "Вехи",
        new MyIcons.Size20.MilestoneRhomb(),
        Color.Neutral,
        "Добавлена(ы) веха(и)",
        "Добавление вех");
        
    public static readonly FeedMsgType Project = new(
        "project",
        "Проекты",
        new Icons.Filled.Size20.BookAdd(),
        Color.Warning,
        "Добавлен проект",
        "Добавление проектов");

    public static readonly FeedMsgType[] Types = [Task, Participant, Milestone, Project];
}

public record FeedMsgType(string Type, string Name, Icon Icon, Color Color, string Header, string? Description = null);