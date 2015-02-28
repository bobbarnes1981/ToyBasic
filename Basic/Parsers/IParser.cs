namespace Basic.Parsers
{
    public interface IParser<T>
    {
        T Parse(ITextStream input);
    }
}
