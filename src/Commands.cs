namespace CodeCrafters.Shell;

internal static class Commands
{
    public static CommandResult Exit(CommandInput _) => CommandResult.Exit;

    public static CommandResult Echo(CommandInput input)
    {
        if (input.Args.Length == 1)
        {
            input.Out.WriteLine();
        }
        else
        {
            input.Out.WriteLine(string.Join(' ', input.Args[1..]));
        }
        
        return CommandResult.Continue;
    }

    public static CommandResult Type(CommandInput input)
    {
        var name = input.Args.Length > 1 ? input.Args[1] : string.Empty;
        var command = Command.Get(name);
        input.Out.WriteLine(command switch
        {
            BuiltInCommand => $"{name} is a shell builtin",
            ExternalCommand c => $"{name} is {c.Path}",
            _ => $"{name}: not found"
        });
        return CommandResult.Continue;
    }

    public static CommandResult Pwd(CommandInput input)
    {
        input.Out.WriteLine(Environment.CurrentDirectory);
        return CommandResult.Continue;
    }
    
    public static CommandResult Cd(CommandInput input)
    {
        if (input.Args.Length == 1)
        {
            return CommandResult.Continue;
        }

        var path = input.Args[1];

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
            input.Out.WriteLine($"cd: {path}: No such file or directory");
        }

        return CommandResult.Continue;
    }
}