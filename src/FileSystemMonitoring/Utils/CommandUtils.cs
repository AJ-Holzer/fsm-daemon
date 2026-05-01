using FileSystemMonitoring.Utils.Models;

namespace FileSystemMonitoring.Utils;

public static class CommandUtils
{
    /// <summary>
    /// Get the CommandType.
    /// </summary>
    /// <param name="commandTypeString"></param>
    /// <returns></returns>
    private static CommandType GetCommandType(string commandTypeString)
    {
        return commandTypeString switch
        {
            "--help" => CommandType.Help,
            "--stop" => CommandType.Stop,
            "--start" => CommandType.Start,
            "--restart" => CommandType.Restart,
            "--pause" => CommandType.Pause,
            "--continue" => CommandType.Continue,
            "--add-folder" => CommandType.AddFolder,
            "--remove-folder" => CommandType.RemoveFolder,
            "--change-naming-convention" => CommandType.ChangeNamingConvention,
            "--add-naming-convention" => CommandType.AddNamingConvention,
            "--update-naming-convention" => CommandType.UpdateNamingConvention,
            "--remove-naming-convention" => CommandType.RemoveNamingConvention,
            _ => CommandType.Help, // Default to help command if wrong command
        };
    }

    /// <summary>
    /// Parse the command string and split args.
    /// </summary>
    /// <param name="commandString"></param>
    /// <returns></returns>
    public static Command ParseCommand(string commandString)
    {
        // Get command parts
        string[] commandParts = commandString.Split();

        // Get command type and args
        string[] args = [.. commandParts[1..].Select(x => x.Trim())];
        CommandType type = GetCommandType(commandTypeString: commandParts[0]);

        return new Command(CommandType: type, Args: args);
    }
}
