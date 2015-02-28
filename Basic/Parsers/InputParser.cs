using System.Linq;

namespace Basic.Parsers
{
    public class InputParser : Parser<object>
    {
        public override object Parse(ITextStream input)
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
