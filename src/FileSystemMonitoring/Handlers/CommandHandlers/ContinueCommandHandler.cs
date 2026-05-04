using FileSystemMonitoring.Handlers.CommandHandlers.Interfaces;

namespace FileSystemMonitoring.Handlers.CommandHandlers;

public class ContinueCommandHandler : ICommandHandler
{
    public IEnumerable<string> Commands => ["start"];

    public string Description => "Starts the daemon.";

    public Task<string?> ExecuteAsync(CancellationToken ct, IEnumerable<string> args)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return nameof(ContinueCommandHandler);
    }
}
