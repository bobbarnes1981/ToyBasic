using Basic.Tokenizer;

namespace Basic.Parser
{
    public interface IParser
    {
        ILine Parse(ITokenCollection tokens);
    }
}
