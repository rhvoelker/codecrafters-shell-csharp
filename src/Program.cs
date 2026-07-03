using System.Reflection;

namespace CodeCrafters.Shell;

internal class Program
{
    private static void Main()
    {
        var result = CommandResult.Continue;
        
        while (!result.ShouldExit)
        {
            Console.Write("$ ");
            var input = Console.ReadLine() ?? string.Empty;
            var args = input.Split(' ');
            var command = GetCommand(args[0]);

            result = command(args);
        }
    }

    private static Func<string[], CommandResult> GetCommand(string name)
    {
        var func = typeof(Commands)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .FirstOrDefault(m => string.Equals(m.Name, name, StringComparison.InvariantCultureIgnoreCase));

        if (func is null)
        {
            return _ =>
            {
                Console.WriteLine("{0}: command not found", name);
                return CommandResult.Continue;
            };
        }

        return (Func<string[], CommandResult>)Delegate.CreateDelegate(typeof(Func<string[], CommandResult>), func);
    }
}

