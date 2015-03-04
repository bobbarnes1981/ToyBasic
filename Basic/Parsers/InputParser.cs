using System.Linq;

namespace Basic.Parsers
{
    public interface IInputParser
    {
        object Parse(ITextStream input);
    }

    // TODO: replace this?
    public class InputParser : IInputParser
    {
        protected readonly char[] NUMBERS =
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
        };

        public object Parse(ITextStream input)
        {
            string text = string.Empty;
            char character;
            bool isNumber = true;
            while(!input.End)
            {
                character = input.Next();
                if (isNumber && !NUMBERS.Contains(character))
                {
                    isNumber = false;
                }

                text += character;
            }

            if (isNumber)
            {
                return int.Parse(text);
            }

            return text;
        }
    }
}
