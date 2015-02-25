namespace Basic.Parsers
{
    public interface IParser<T>
    {
        T Parse(IInterpreter interpreter, ITextStream input);
    }
}
