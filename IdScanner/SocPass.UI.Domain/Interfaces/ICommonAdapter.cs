using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.UI.Domain.Interfaces
{
    public interface ICommonAdapter
    {
        Task<T> GetAsync<T>(string EndPoint);
        Task<T> PostAsync<T>(string endpoint, object? data = null);
        Task<string> PutAsync<TRequest>(string endpoint, TRequest data);
        Task<string> DeleteAsync<TResponse>(string endpoint);
        //Task<TResponse> addUpdateMemberAndGuestAsync<TRequest, TResponse>(string endpoint, TRequest data);

    }
}
