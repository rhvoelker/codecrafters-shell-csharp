using System.Text;
using Antlr4.Runtime.Tree;

namespace CodeCrafters.Shell.ArgParsing;

internal class ArgListener : CommandBaseListener
{
    private readonly List<string> _args = [];
    
    public string[] Args => _args.ToArray();
    
    public override void EnterArg(CommandParser.ArgContext context)
    {
        _args.Add(string.Join(string.Empty, context
            .UNQUOTED()
            .Union(context.SSTRING())
            .Union(context.DSTRING())
            .Union(context.ESCAPE())
            .OrderBy(token => token.Symbol.TokenIndex)
            .Select<ITerminalNode, string>(token => token.Symbol.Type switch
            {
                CommandParser.UNQUOTED => token.GetText(),
                CommandParser.SSTRING or CommandParser.DSTRING => UnwrapQuotedString(token.GetText()),
                CommandParser.ESCAPE => Unescape(token.GetText()),
                _ => string.Empty
            })));
    }
    
    private static string UnwrapQuotedString(string s) => s.Length >= 2
        ? s.Substring(1, s.Length - 2)
        : s;
    
    private static string Unescape(string s) => s.Length > 1 ? s[1..] : s;
}