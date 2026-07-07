using System.Text.RegularExpressions;

namespace CodeCrafters.Shell.ArgParsing;

internal partial class ArgListener : CommandBaseListener
{
    private static Regex QuoteRegex = ExecQuoteRegex();

    private readonly List<string> _args = [];
    
    public string[] Args => _args.ToArray();
    
    public override void EnterArg(CommandParser.ArgContext context)
    {
        var token = context
            .SSTRING()
            ?.GetText()
            .Replace("'", string.Empty);
        
        token ??= QuoteRegex.Replace(
            context.UNQUOTED()?.GetText() ?? string.Empty,
            string.Empty);

        _args.Add(token);
    }

    [GeneratedRegex(@"['""]+")]
    private static partial Regex ExecQuoteRegex();
}