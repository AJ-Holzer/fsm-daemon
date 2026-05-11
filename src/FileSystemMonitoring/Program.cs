using FileSystemMonitoring.Services;
using FileSystemMonitoring.Services.Interfaces;
using FileSystemMonitoring.Shared.Models.Config;
using YamlDotNet.Serialization;

namespace FileSystemMonitoring;

internal static class Program
{
    internal static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        // Load config file
        AppConfig appConfig = LoadAppConfig("/home/$USER/.config/fsmdaemon/config.yaml");

        builder.Services.AddSingleton<IService, FSRenameService>();
        builder.Services.AddSingleton(appConfig);
        builder.Services.AddHostedService<Worker>();

        var host = builder.Build();
        host.Run();
    }

    /// <summary>
    /// Loads the YAML config file and returns an AppConfig object containing all config data.
    /// </summary>
    /// <param name="filepath"></param>
    /// <returns></returns>
    private static AppConfig LoadAppConfig(string filepath)
    {
        // Create deserializer
        IDeserializer deserializer = new DeserializerBuilder().Build();

        // Read yaml content
        string yamlContent = File.ReadAllText(filepath);

        // Deserialize content into app config object
        AppConfig appConfig = deserializer.Deserialize<AppConfig>(yamlContent);

        return appConfig;
    }
}
