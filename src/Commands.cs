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
}