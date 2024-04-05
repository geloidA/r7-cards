namespace Cardmngr.Notification;

public class NotificationOptions
{
    public string? FluentBadge { get; set; }

    public string? Body { get; set; }

    public object? Data { get; set; }

    // type NotificationDirection = "auto" | "ltr" | "rtl";
    public string? Dir { get; set; } = "auto";

    public string? Icon { get; set; }

    public string? Lang { get; set; } = "ru";

    public bool? RequireInteraction { get; set; }

    public bool? Silent { get; set; }

    public string? Image { get; set; }

    public string? Tag { get; set; }

    public string? Href { get; set; }

    public bool? Renotify { get; set; }

    public int? TimeOut { get; set; } = 500;
}