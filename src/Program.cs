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
            var command = Command.Get(args[0]);

            result = command.Invoke(args);
        }
    }
}

