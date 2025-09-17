namespace SocPass.UI.Application.Interface
{
    public interface IForgotPasswordService
    {
        Task<string> ForgotPasswordAsync(string email);
    }
}
