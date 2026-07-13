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
        if (listener.OutputFileStream != null)
        {
            outWriter = new StreamWriter(listener.OutputFileStream);
        }
        
        var errorWriter = Console.Error;
        if (listener.ErrorFileStream != null)
        {
            errorWriter = new StreamWriter(listener.ErrorFileStream);
        }

        return new CommandInput(
            outWriter,
            errorWriter,
            listener.Args);
    }
}