using SocPass.Application.Interface;
using SocPass.Domain.Interface;

namespace SocPass.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }
        public async Task<object> GetReportAsync(int blockId, int eventId, DateTime startDate)
        {
            return await _reportRepository.GetFlatAttendanceReportAsync(blockId, eventId, startDate);
        }
    }
}
