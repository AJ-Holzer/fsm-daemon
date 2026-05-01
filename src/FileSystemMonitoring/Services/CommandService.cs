using System.Data;
using System.IO.Pipes;
using System.Threading.Channels;
using FileSystemMonitoring.Utils;
using FileSystemMonitoring.Utils.Models;

namespace FileSystemMonitoring.Services;

public class CommandService(string pipeName)
{
    private readonly string _pipeName = pipeName;

    private async Task<string> ProcessCommand(CancellationToken ct, string command)
    {
        Command parsedCommand = CommandUtils.ParseCommand(commandString: command);

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
}
