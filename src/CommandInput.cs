namespace CodeCrafters.Shell;

internal record CommandInput(
    TextWriter Out,
    TextWriter Error,
    string[] Args)
{
    public void TryCloseWriters()
    {
        if (Out != Console.Out)
        {
            Out.Close();
        }
        
        if (Error != Console.Error)
        {
            Error.Close();
        }
    }
}