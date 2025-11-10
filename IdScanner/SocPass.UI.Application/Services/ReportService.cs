using SocPass.UI.Application.Interface;
using SocPass.UI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocPass.UI.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportAdapter _reportRepository;
        public ReportService(IReportAdapter reportRepository)
        {
            _reportRepository = reportRepository;
        }
        public async Task<object> GetReportAsync(int blockId, int eventId, DateTime startDate)
        {
            return await _reportRepository.GetReportAsync(blockId, eventId, startDate);
        }
    }
}
