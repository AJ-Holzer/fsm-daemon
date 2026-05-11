namespace FileSystemMonitoring.Shared.Models.Config;

public class AppConfig
{
    public string NamingRegex { get; set; } = "[^a-z0-9\\.]";
    public string Separator { get; set; } = "_";
    public bool LowercaseExtension { get; set; } = true;
    public int RenameDelay { get; set; } = 100;
    public IEnumerable<string> IncludedPaths { get; set; } = [];
    public IEnumerable<string> ExcludedPaths { get; set; } = [];
    public IEnumerable<string> ExcludedFilenames { get; set; } = [];
}
