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
        Console.WriteLine(command.Type switch
        {
            CommandType.BuiltIn => $"{name} is a shell builtin",
            CommandType.External => $"{name} is {command.Path}",
            _ => $"{name}: not found"
        });
        return CommandResult.Continue;
    }
}