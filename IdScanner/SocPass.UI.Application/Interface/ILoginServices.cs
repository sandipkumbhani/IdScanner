using SocPass.UI.Domain.Model;

namespace SocPass.UI.Application.Interface
{
    public interface ILoginServices
    {
        Task<ResponseToken> Login(LoginViewModel model);
    }
}
