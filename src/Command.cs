using System.Diagnostics;
using System.Reflection;

namespace CodeCrafters.Shell;

internal abstract class Command
{
    public static Command Get(string name) =>
        GetBuiltInCommand(name)
            ?? GetExternalCommand(name)
            ?? new NotFoundCommand(name);

    public abstract CommandResult Invoke(CommandInput input);

    private static Command? GetBuiltInCommand(string name)
    {
        var func = typeof(Commands)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .FirstOrDefault(m => string.Equals(m.Name, name, StringComparison.InvariantCultureIgnoreCase));

        return func is not null
            ? new BuiltInCommand(
                (Func<CommandInput, CommandResult>)
                    Delegate.CreateDelegate(typeof(Func<CommandInput, CommandResult>), func))
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

internal class BuiltInCommand(Func<CommandInput, CommandResult> func) : Command
{
    public override CommandResult Invoke(CommandInput input) => func(input);
}

internal class ExternalCommand(string path) : Command
{
    public string Path { get; } = path;

    public override CommandResult Invoke(CommandInput input)
    {
        using var process = new Process();
        process.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(Path);
        process.StartInfo.FileName = System.IO.Path.GetFileName(Path);
        process.StartInfo.Arguments = input.Args.Length > 1
            ? string.Join(' ', input.Args[1..].Select(QuoteAndEscapeDoubleQuotes))
            : string.Empty;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.OutputDataReceived += (_, line) =>
        {
            if (!string.IsNullOrEmpty(line.Data))
            {
                input.Out.WriteLine(line.Data);
            }
        };
        process.StartInfo.RedirectStandardError = true;
        process.ErrorDataReceived += (_, line) =>
        {
            if (!string.IsNullOrEmpty(line.Data))
            {
                input.Error.WriteLine(line.Data);
            }
        };
        process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        process.StartInfo.CreateNoWindow = true;
        process.Start();
        
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit();
        
        return CommandResult.Continue;
    }

    private static string QuoteAndEscapeDoubleQuotes(string s) => s.EndsWith('\\')
        ? $"\"{s.Replace("\"", "\\\"")}\\\""
        : $"\"{s.Replace("\"", "\\\"")}\"";
}

internal class NotFoundCommand(string name) : Command
{
    public override CommandResult Invoke(CommandInput _)
    {
        Console.WriteLine("{0}: command not found", name);
        return CommandResult.Continue;
    }
}