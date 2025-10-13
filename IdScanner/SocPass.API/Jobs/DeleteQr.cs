using Quartz;

namespace SocPass.API.Jobs
{
    public class DeleteQr : IJob
    {
        private readonly ILogger<DeleteQr> _logger;

        public DeleteQr(ILogger<DeleteQr> logger)
        {
            _logger = logger;
        }
        public Task Execute(IJobExecutionContext context)
        {
            string qrBasePath = Path.Combine(@"D:\Broadsy\Projects\IdScanner\IdScanner\SocPass.UI", "wwwroot", "QRCodes");
            _logger.LogInformation($"[QR Cleanup Job] Running at {DateTime.Now}");

            try
            {
                if (!Directory.Exists(qrBasePath))
                {
                    _logger.LogWarning($"QR base path not found: {qrBasePath}");
                    return Task.CompletedTask;
                }
                foreach (var dateFolder in Directory.GetDirectories(qrBasePath, "*", SearchOption.TopDirectoryOnly))
                {
                    string folderName = Path.GetFileName(dateFolder);
                    _logger.LogInformation($"Checking folder: {folderName}");

                    if (DateTime.TryParseExact(folderName, "dd-MM-yyyy",
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None, out DateTime passDate))
                    {
                        if (passDate <= DateTime.Now.Date.AddDays(-2))
                        {
                            Directory.Delete(dateFolder, true);
                            _logger.LogInformation($" Deleted old QR folder: {dateFolder}");
                        }
                        else
                        {
                            _logger.LogInformation($"Skipping folder (still valid): {folderName}");
                        }
                    }
                    else
                    {
                        _logger.LogInformation($"Skipping non-date folder: {folderName}");
                    }
                } 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[QR Cleanup Job] Error deleting old QR folders.");
            }

            return Task.CompletedTask;
        }
    }
}

