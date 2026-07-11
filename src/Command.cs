using System.Diagnostics;
using System.Reflection;

namespace CodeCrafters.Shell;

internal abstract class Command
{
    public static Command Get(string name) =>
        GetBuiltInCommand(name)
            ?? GetExternalCommand(name)
            ?? new NotFoundCommand(name);

    public abstract CommandResult Invoke(string[] args);

    private static Command? GetBuiltInCommand(string name)
    {
        var func = typeof(Commands)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .FirstOrDefault(m => string.Equals(m.Name, name, StringComparison.InvariantCultureIgnoreCase));

        return func is not null
            ? new BuiltInCommand(
                (Func<string[], CommandResult>)Delegate.CreateDelegate(typeof(Func<string[], CommandResult>), func))
            : null;
    }

    private static Command? GetExternalCommand(string name)
    {
        var path = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
        var paths = path.Split(':');
        var commandPath = paths
            .Select(p => Path.Join(p, name))
            .Where(File.Exists)
            .FirstOrDefault(FilePermissionsHelper.CanExecute);

        return !string.IsNullOrEmpty(commandPath)
            ? new ExternalCommand( commandPath)
            : null;
    }
}

internal class BuiltInCommand(Func<string[], CommandResult> func) : Command
{
    public override CommandResult Invoke(string[] args) => func(args);
}

internal class ExternalCommand(string path) : Command
{
    public string Path { get; } = path;

    public override CommandResult Invoke(string[] args)
    {
        using var process = new Process();
        process.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(Path);
        process.StartInfo.FileName = System.IO.Path.GetFileName(Path);
        process.StartInfo.Arguments = args.Length > 1
            ? string.Join(' ', args[1..].Select(QuoteArg))
            : string.Empty;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        process.StartInfo.CreateNoWindow = true;
        process.Start();

        while (!process.StandardOutput.EndOfStream)
        {
            Console.WriteLine(process.StandardOutput.ReadLine());
        }
                    
        process.WaitForExit();
                    
        return CommandResult.Continue;
    }

    private static string QuoteArg(string arg) => arg.EndsWith('\\')
        ? $"\"{arg}\\\""
        : $"\"{arg}\"";
}

internal class NotFoundCommand(string name) : Command
{
    public override CommandResult Invoke(string[] args)
    {
        Console.WriteLine("{0}: command not found", name);
        return CommandResult.Continue;
    }
}