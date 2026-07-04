namespace CodeCrafters.Shell;

internal static class Commands
{
    public static CommandResult Exit(string[] args) => CommandResult.Exit;

    public static CommandResult Echo(string[] args)
    {
        if (args.Length == 1)
        {
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine(string.Join(' ', args[1..]));
        }
        
        return CommandResult.Continue;
    }

    public static CommandResult Type(string[] args)
    {
        var name = args.Length > 1 ? args[1] : string.Empty;
        var command = Command.Get(name);
        Console.WriteLine(command switch
        {
            BuiltInCommand => $"{name} is a shell builtin",
            ExternalCommand c => $"{name} is {c.Path}",
            _ => $"{name}: not found"
        });
        return CommandResult.Continue;
    }

    public static CommandResult Pwd(string[] args)
    {
        Console.WriteLine(Environment.CurrentDirectory);
        return CommandResult.Continue;
    }
    
    public static CommandResult Cd(string[] args)
    {
        if (args.Length == 1)
        {
            return CommandResult.Continue;
        }

        var path = args[1];

        if (path.StartsWith("~"))
        {
            path = Environment.GetEnvironmentVariable("HOME") + path[1..];
        }

        if (Directory.Exists(path))
        {
            Environment.CurrentDirectory = path;
        }
        else
        {
            Console.WriteLine($"cd: {path}: No such file or directory");
        }

        return CommandResult.Continue;
    }
}