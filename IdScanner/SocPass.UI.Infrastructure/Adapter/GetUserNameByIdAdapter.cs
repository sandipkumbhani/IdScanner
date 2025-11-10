using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SocPass.Domain.Model;
using SocPass.UI.Domain.Helper;
using SocPass.UI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.UI.Infrastructure.Provider
{
    public class GetUserNameByIdAdapter : IGetUserNameByIdAdapter
    {
        private readonly ICommonAdapter _commonAdapter;
        public GetUserNameByIdAdapter(ICommonAdapter commonAdapter)
        {
            _commonAdapter = commonAdapter;
        }
        public async Task<User> GetUserNameAsync(int userId)
        {
            return await _commonAdapter.GetAsync<User>($"GetUserName/get-user-name?userId={userId}");
        }
    }
}
