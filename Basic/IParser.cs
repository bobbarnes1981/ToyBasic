namespace Basic
{
    public interface IParser
    {
        Line Parse(IInterpreter interpreter, ITextStream input);
    }
}
