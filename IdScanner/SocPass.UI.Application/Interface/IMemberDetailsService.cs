namespace SocPass.UI.Application.Interface
{
    public interface IMemberDetailsService
    {
        Task<bool> IsVisitedAsync(int memberid, int loggedInUserId);
    }
}
