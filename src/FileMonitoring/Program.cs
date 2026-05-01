namespace FileMonitoring;

internal static class Program
{
    internal static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddHostedService<Worker>();

        var host = builder.Build();
        host.Run();

        FileSystemWatcher fsw = new(path: "./", filter: "*.txt");
    }
}
