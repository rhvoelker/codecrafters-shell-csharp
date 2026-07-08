using System.Text;
using System.Text.RegularExpressions;

namespace CodeCrafters.Shell.ArgParsing;

internal partial class ArgListener : CommandBaseListener
{
    private static Regex QuoteRegex = ExecQuoteRegex();

    private readonly List<string> _args = [];
    
    public string[] Args => _args.ToArray();
    
    public override void EnterArg(CommandParser.ArgContext context)
    {
        var arg = new StringBuilder();
        
        foreach (var unquoted in context.UNQUOTED())
        {
            arg.Append(unquoted.GetText());
        }

        foreach (var singleQuoted in context.SSTRING())
        {
            arg.Append(singleQuoted.GetText().Replace("'", string.Empty));
        }

        foreach (var doubleQuoted in context.DSTRING())
        {
            arg.Append(doubleQuoted.GetText().Replace("\"", string.Empty));
        }

        _args.Add(arg.ToString());
    }

    [GeneratedRegex(@"['""]+")]
    private static partial Regex ExecQuoteRegex();
}