using System.Reflection;

namespace CodeCrafters.Shell;

internal class Command
{
    public static Command Get(string name)
    {
        var func = typeof(Commands)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .FirstOrDefault(m => string.Equals(m.Name, name, StringComparison.InvariantCultureIgnoreCase));

        if (func is null)
        {
            return new Command(
                false,
                _ =>
                {
                    Console.WriteLine("{0}: command not found", name);
                    return CommandResult.Continue;
                });
        }

        return new Command(
            true,
            (Func<string[], CommandResult>)Delegate.CreateDelegate(typeof(Func<string[], CommandResult>), func));
    }

    private Command(bool exists, Func<string[], CommandResult> func)
    {
        Exists = exists;
        Invoke = func;
    }
    
    public bool Exists { get; }
    public Func<string[], CommandResult> Invoke { get; }
}