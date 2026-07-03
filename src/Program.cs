namespace CodeCrafters.Shell;

internal class Program
{
    private static void Main()
    {
        Console.Write("$ ");
        var command = Console.ReadLine();
        Console.WriteLine("{0}: command not found", command);
    }
}