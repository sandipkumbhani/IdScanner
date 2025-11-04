using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.UI.Application.Interface
{
    public interface IReportService
    {
        Task<object> GetReportAsync(int blockId, int eventId, DateTime startDate);
    }
}
