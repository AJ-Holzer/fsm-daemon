using FileSystemMonitoring.Services.Interfaces;

namespace FileSystemMonitoring;

public class Worker(IService fsRenameService) : BackgroundService
{
    private readonly IService _fsRenameService = fsRenameService;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _fsRenameService.RunAsync(ct: stoppingToken);
    }
}
