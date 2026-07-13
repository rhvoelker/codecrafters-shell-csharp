using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using CodeCrafters.Shell.ArgParsing;

namespace CodeCrafters.Shell;

internal static class CommandInputEvaluator
{
    public static CommandInput Eval(string input)
    {
        var lexer = new CommandLexer(new AntlrInputStream(input));
        var parser = new CommandParser(new CommonTokenStream(lexer));

        var listener = new CommandInputListener();
        ParseTreeWalker.Default.Walk(listener, parser.cmd());
        
        var outWriter = Console.Out;

        if (listener.OutputFilePath != null)
        {
            var directoryName = Path.GetDirectoryName(listener.OutputFilePath);
            if (!string.IsNullOrEmpty(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            
            outWriter = new StreamWriter(File.Create(listener.OutputFilePath));
        }
        
        var errorWriter = Console.Error;

        if (listener.ErrorFilePath != null)
        {
            var directoryName = Path.GetDirectoryName(listener.ErrorFilePath);
            if (!string.IsNullOrEmpty(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            
            errorWriter = new StreamWriter(File.Create(listener.ErrorFilePath));
        }

        return new CommandInput(
            outWriter,
            errorWriter,
            listener.Args);
    }
}