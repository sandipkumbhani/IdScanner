using SocPass.UI.Domain.Model;

namespace SocPass.UI.Application.Interface
{
    public interface IResetPasswordService
    {
        Task<string> ResetPassworsdAsync(ResetPasswordModel resetPasswordModel);
    }
}
