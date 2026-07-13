using System.Text;

namespace CodeCrafters.Shell.ArgParsing;

internal class CommandInputListener : CommandBaseListener
{
    private readonly List<string> _args = [];
    private readonly StringBuilder _currentArg = new();
    private Action _exitArgAction;
    
    public string[] Args => _args.ToArray();
    
    public string? OutputFilePath { get; private set; }

    public CommandInputListener()
    {
        _exitArgAction = AddCurrentArgToArgsList;
    }
    
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

    public override void ExitArg(CommandParser.ArgContext context) => _exitArgAction.Invoke();
    
    public override void EnterRed(CommandParser.RedContext context) => _exitArgAction = SetOutputFilePathToCurrentArg;

    public override void ExitRed_stream(CommandParser.Red_streamContext context) =>
        _exitArgAction = context.GetText() switch
        {
            "1" => SetOutputFilePathToCurrentArg,
            { } s => throw new Exception($"{s} is not a known output stream for redirection.")
        };
    
    public override void ExitRed(CommandParser.RedContext context) => _exitArgAction = AddCurrentArgToArgsList;

    private void AddCurrentArgToArgsList() => _args.Add(_currentArg.ToString());
    
    private void SetOutputFilePathToCurrentArg() => OutputFilePath = _currentArg.ToString();

    private static string Unescape(string s) => s.Length > 1 ? s[1..] : s;
}