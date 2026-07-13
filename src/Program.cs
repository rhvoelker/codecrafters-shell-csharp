namespace CodeCrafters.Shell;

internal class Program
{
    private static void Main()
    {
        var result = CommandResult.Continue;
        
        while (!result.ShouldExit)
        {
            Console.Write("$ ");
            var input = CommandInputEvaluator.Eval(Console.ReadLine() ?? string.Empty);

            try
            {
                result = Command.Get(input.Args[0]).Invoke(input);
            }
            catch (Exception e)
            {
                input.Error.WriteLine(e);
            }
            finally
            {
                input.TryCloseWriters();
            }
        }
    }
}

