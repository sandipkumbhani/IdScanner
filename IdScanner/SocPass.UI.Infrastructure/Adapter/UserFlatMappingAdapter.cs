using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SocPass.Domain.Model;
using SocPass.UI.Domain.Helper;
using SocPass.UI.Domain.Interfaces;
using SocPass.UI.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.UI.Infrastructure.Provider
{
    public class UserFlatMappingAdapter : IUserFlatMappingAdapter
    {
        private readonly ICommonAdapter _commonAdapter;
        public UserFlatMappingAdapter(ICommonAdapter commonAdapter)
        {
            _commonAdapter = commonAdapter;
        }
        public async Task<IList<QRCodeMaster>> GetQrByUserId(int? userid,int EventId)
        {
            return await _commonAdapter.GetAsync<IList<QRCodeMaster>>($"UserFlatMapping/GetQrByUserId?userid={userid}&EventId={EventId}");
        }
    }
}
