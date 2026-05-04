using System.IO.Pipes;
using FileSystemMonitoring.Handlers.CommandHandlers.Interfaces;

namespace FileSystemMonitoring.Services;

public class CommandService(string pipeName, IEnumerable<ICommandHandler> commandHandlers)
{
    private readonly string _pipeName = pipeName;
    private readonly IEnumerable<ICommandHandler> _commandHandlers = commandHandlers;

    private IDictionary<string, ICommandHandler> _indexedHandlers;

    /// <summary>
    /// Index handlers using their commands.
    /// </summary>
    /// <param name="ct"></param>
    /// <exception cref="InvalidOperationException"></exception>
    private void IndexHandlers()
    {
        // Index handlers
        foreach (ICommandHandler handler in _commandHandlers)
        {
            foreach (string command in handler.Commands)
            {
                // Check for duplicate command
                if (_indexedHandlers.ContainsKey(command))
                    throw new InvalidOperationException(
                        $"Command '{command}' does already exist in '{_indexedHandlers[command]}'!"
                    );

                // Index handler
                _indexedHandlers[command] = handler;
            }
        }
    }

    private async Task<string> ProcessCommand(CancellationToken ct, string command)
    {
        return "";
    }

    private async Task CreatePipeAndHandleCommands(CancellationToken ct)
    {
        // Create pipe
        NamedPipeServerStream pipe = new(
            pipeName: _pipeName,
            direction: PipeDirection.InOut,
            maxNumberOfServerInstances: 1,
            transmissionMode: PipeTransmissionMode.Byte,
            options: PipeOptions.Asynchronous
        );

        // Wait for connections
        await pipe.WaitForConnectionAsync(cancellationToken: ct);

        // TODO: Check if this really works
        using StreamReader reader = new(pipe);
        using StreamWriter writer = new(pipe);

        while (!ct.IsCancellationRequested)
        {
            // Receive command
            string? command = await reader.ReadLineAsync();

            // Skip if command is null
            if (string.IsNullOrWhiteSpace(value: command))
                continue;

            // Process command and send answer
            string answer = await ProcessCommand(ct: ct, command: command);
            await writer.WriteLineAsync(value: answer);
        }
    }

    public async Task Listen(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            try
            {
                await CreatePipeAndHandleCommands(ct: ct);
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                // Expected exit
                break;
            }
        }
    }

    public async Task StartSync(CancellationToken ct)
    {
        IndexHandlers();
        await Listen(ct: ct);
    }
}
