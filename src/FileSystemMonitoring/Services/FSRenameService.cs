using System.Text.RegularExpressions;
using FileSystemMonitoring.Services.Interfaces;
using FileSystemMonitoring.Shared.Models.Config;

namespace FileSystemMonitoring.Services;

public class FSRenameService(AppConfig appConfig) : IService
{
    private const NotifyFilters NOTIFY_FILTERS =
        NotifyFilters.FileName | NotifyFilters.DirectoryName;

    // Compile regex here to avoid recompiling every time it is used
    private readonly Regex _filenameRegex = new(appConfig.NamingRegex);
    private readonly string _separator = appConfig.Separator;
    private readonly bool _lowercaseExtension = appConfig.LowercaseExtension;
    private readonly IEnumerable<string> _includedPaths = appConfig.IncludedPaths;
    private readonly IEnumerable<string> _excludedPaths = appConfig.ExcludedPaths;
    private readonly IEnumerable<string> _excludedFilenames = appConfig.ExcludedFilenames;
    private readonly int _renameDelay = appConfig.RenameDelay;

    private readonly Regex _multipleSeparatorRegex = new($"{appConfig.Separator}{{2,}}");
    private readonly IList<FileSystemWatcher> _fsWatchers = [];

    /// <summary>
    /// Get the new filename based on the regex specified in the initialization.
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="isFile"></param>
    /// <returns></returns>
    private string GetNewFilename(string filename, bool isFile)
    {
        // Get name and extension
        string name = Path.GetFileNameWithoutExtension(filename);
        string extension = Path.GetExtension(filename);

        // Lower extension if specified
        if (_lowercaseExtension)
            extension = extension.ToLower();

        // Apply regex to replace unwanted characters with the separator
        string sanitizedName = _filenameRegex.Replace(
            input: isFile ? name.ToLower() : filename.ToLower(),
            replacement: _separator
        );

        // Replace multiple separators
        sanitizedName = _multipleSeparatorRegex.Replace(
            input: sanitizedName,
            replacement: _separator
        );

        // Trim sanitized name
        sanitizedName = sanitizedName.Trim(_separator).ToString();

        return isFile ? (sanitizedName + extension) : sanitizedName;
    }

    /// <summary>
    /// Rename a file or folder.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void Rename(object sender, FileSystemEventArgs e)
    {
        // Get the filename and check if the path is a file
        bool isFile = File.Exists(e.FullPath);
        string filename = Path.GetFileName(e.FullPath);
        string parentPath = Path.GetDirectoryName(e.FullPath) ?? "./";
        string parentPathName = Path.GetFileName(parentPath);

        // Skip if filename is excluded
        if (_excludedFilenames.Contains(filename))
            return;

        // Skip if path is excluded
        // TODO: Improve it for checking paths like '~/' too!
        if (_excludedPaths.Contains(parentPathName) || _excludedPaths.Contains(parentPath))
            return;

        // Get new filename
        string newFilename = GetNewFilename(filename: filename, isFile: isFile);

        // Skip if old and new filename is the same
        if (filename == newFilename)
            return;

        // Get new filepath
        string newFullPath = Path.Combine(parentPath, newFilename);

        // Wait for the specified rename delay to avoid errors with other programs
        await Task.Delay(_renameDelay);

        // Rename file or folder
        await Task.Run(() =>
        {
            if (isFile)
                File.Move(e.FullPath, newFullPath);
            else
                Directory.Move(e.FullPath, newFullPath);
        });
    }

    /// <summary>
    /// Create watchers and add them to the internal watchers list.
    /// </summary>
    private void BuildWatchers()
    {
        foreach (string path in _includedPaths)
        {
            // Create watcher
            FileSystemWatcher fsw = new(path: Path.GetFullPath(path))
            {
                IncludeSubdirectories = true,
                NotifyFilter = NOTIFY_FILTERS,
            };

            // Rename when file is created, renamed or moved
            fsw.Created += Rename;
            fsw.Renamed += Rename;
            fsw.Changed += Rename;

            // Enable raising events
            fsw.EnableRaisingEvents = true;

            // Add watcher
            _fsWatchers.Add(fsw);
        }
    }

    /// <summary>
    /// Build watchers and rename files if needed. Wait until cancellation is requested.
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task RunAsync(CancellationToken ct)
    {
        BuildWatchers();
        await Task.Delay(Timeout.Infinite, ct);

        // Dispose everything after cancellation is requested
        foreach (FileSystemWatcher watcher in _fsWatchers)
            watcher.Dispose();
    }
}
