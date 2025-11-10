namespace SocPass.UI.Application.Interface
{
    public interface IMemberDetailsService
    {
        Task<string> IsVisitedAsync(int memberid, int EventId,int loggedInUserId);
    }
}
