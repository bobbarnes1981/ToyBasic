namespace Basic
{
    public interface IParser
    {
        Line Parse(IInterpreter interpreter, string input);
    }
}
