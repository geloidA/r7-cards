namespace Cardmngr.Application.Clients.SignalRHubClients
{
    public class MembersChangedEventArgs(MemberAction action, string userId) : EventArgs
    {
        public MemberAction Action { get; set; } = action;
        public string UserId { get; set; } = userId;
    }
}