using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.Domain.Interface
{
    public interface IReportRepository
    {
        Task<object> GetFlatAttendanceReportAsync(int blockId, int eventId, DateTime startDate);
    }
}
