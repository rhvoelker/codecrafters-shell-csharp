using System.Text;
using Antlr4.Runtime.Tree;

namespace CodeCrafters.Shell.ArgParsing;

internal class ArgListener : CommandBaseListener
{
    private readonly List<string> _args = [];
    private readonly StringBuilder _currentArg = new();
    
    public string[] Args => _args.ToArray();
    
    public override void EnterArg(CommandParser.ArgContext context) => _currentArg.Clear();

    public override void ExitEscapeCharacter(CommandParser.EscapeCharacterContext context) =>
        _currentArg.Append(Unescape(context.GetText()));

    public override void ExitUnquotedString(CommandParser.UnquotedStringContext context) =>
        _currentArg.Append(context.GetText());

    public override void ExitSingleStringText(CommandParser.SingleStringTextContext context) =>
        _currentArg.Append(context.GetText());

    public override void ExitDoubleStringEscapeCharacter(CommandParser.DoubleStringEscapeCharacterContext context) =>
        _currentArg.Append(Unescape(context.GetText()));

    public override void ExitDoubleStringText(CommandParser.DoubleStringTextContext context) =>
        _currentArg.Append(context.GetText());

    public override void ExitArg(CommandParser.ArgContext context) =>
        _args.Add(_currentArg.ToString());
    
    private static string Unescape(string s) => s.Length > 1 ? s[1..] : s;
}