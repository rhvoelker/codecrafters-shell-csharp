using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using CodeCrafters.Shell.ArgParsing;

namespace CodeCrafters.Shell;

internal static class ArgEvaluator
{
    public static string[] Eval(string input)
    {
        var lexer = new CommandLexer(new AntlrInputStream(input));
        var parser = new CommandParser(new CommonTokenStream(lexer));

        var listener = new ArgListener();
        ParseTreeWalker.Default.Walk(listener, parser.cmd());
        return listener.Args;
    }
}