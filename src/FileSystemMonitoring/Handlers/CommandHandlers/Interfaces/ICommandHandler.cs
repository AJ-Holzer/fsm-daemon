namespace FileSystemMonitoring.Handlers.CommandHandlers.Interfaces;

public interface ICommandHandler
{
    public IEnumerable<string> Commands { get; }
    public string Description { get; }

    public Task<string?> ExecuteAsync(CancellationToken ct, IEnumerable<string> args);
    public string ToString();
}
