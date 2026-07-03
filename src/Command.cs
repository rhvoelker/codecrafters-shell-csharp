using System.Reflection;

namespace CodeCrafters.Shell;

internal class Command
{
    public static Command Get(string name) => 
        GetBuiltInCommand(name) 
            ?? GetExternalCommand(name)
            ?? GetNotFoundCommand(name);

    private static Command? GetBuiltInCommand(string name)
    {
        var func = typeof(Commands)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .FirstOrDefault(m => string.Equals(m.Name, name, StringComparison.InvariantCultureIgnoreCase));

        return func is not null
            ? new Command(
                CommandType.BuiltIn,
                (Func<string[], CommandResult>)Delegate.CreateDelegate(typeof(Func<string[], CommandResult>), func))
            : null;
    }

    private static Command? GetExternalCommand(string name)
    {
        var path = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
        var paths = path.Split(':');
        var commandPath = paths
            .Select(p => System.IO.Path.Join(p, name))
            .Where(File.Exists)
            .FirstOrDefault(FilePermissionsHelper.CanExecute);

        return !string.IsNullOrEmpty(commandPath)
            ? new Command(
                CommandType.External,
                _ => CommandResult.Continue,
                commandPath)
            : null;
    }
    
    private static Command GetNotFoundCommand(string name) => new(
        CommandType.NotFound,
        _ =>
        {
            Console.WriteLine("{0}: command not found", name);
            return CommandResult.Continue;
        });

    private Command(CommandType type, Func<string[], CommandResult> func)
    {
        Type = type;
        Invoke = func;
    }

    private Command(CommandType type, Func<string[], CommandResult> func, string path) : this(type, func)
    {
        Path = path;
    }
    
    public CommandType Type { get; }
    public Func<string[], CommandResult> Invoke { get; }
    public string Path { get; } = string.Empty;
}

internal enum CommandType
{
    NotFound,
    BuiltIn,
    External
}