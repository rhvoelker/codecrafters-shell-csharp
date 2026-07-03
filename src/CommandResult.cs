namespace CodeCrafters.Shell;

internal class CommandResult
{
    public static readonly CommandResult Continue = new (false);
    public static readonly CommandResult Exit = new (true);
    
    private CommandResult(bool shouldExit) => ShouldExit = shouldExit;
    
    public bool ShouldExit { get; }
}