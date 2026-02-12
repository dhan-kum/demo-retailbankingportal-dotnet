using System.IO.Compression;

namespace RetailBankingPortal.Services;

public class FileService
{
    private readonly ILogger<FileService> _logger;

    public FileService(ILogger<FileService> logger)
    {
        _logger = logger;
    }

    public async Task<bool> CreateZipFile(string sourceDir, string zipFile)
    {
        try
        {
            if (!Directory.Exists(sourceDir))
            {
                _logger.LogWarning("Source directory does not exist: {SourceDir}", sourceDir);
                return false;
            }

            var zipPath = Path.GetDirectoryName(zipFile);
            if (!string.IsNullOrEmpty(zipPath) && !Directory.Exists(zipPath))
            {
                Directory.CreateDirectory(zipPath);
            }

            await Task.Run(() => ZipFile.CreateFromDirectory(sourceDir, zipFile));
            
            _logger.LogInformation("ZIP file created successfully: {ZipFile}", zipFile);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating ZIP file");
            return false;
        }
    }
}
