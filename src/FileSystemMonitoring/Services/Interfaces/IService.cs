namespace FileSystemMonitoring.Services.Interfaces;

public interface IService
{
    public Task RunAsync(CancellationToken ct);
}
