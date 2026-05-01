namespace FileSystemMonitoring.Utils.Models;

public enum CommandType : byte
{
    Help,
    Stop,
    Start,
    Restart,
    Pause,
    Continue,
    AddFolder,
    RemoveFolder,
    ChangeNamingConvention,
    AddNamingConvention,
    UpdateNamingConvention,
    RemoveNamingConvention,
}
