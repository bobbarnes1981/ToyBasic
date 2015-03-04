using NUnit.Framework;
using Basic.Tokenizer;

namespace Basic.UnitTests.Tokenizer
{
    [TestFixture]
    public class TokenizerTests
    {
        [Test]
        public void Tokenizer_Tokenize_Text()
        {
            Basic.Tokenizer.Tokenizer underTest = new Basic.Tokenizer.Tokenizer();

            ITokenCollection tokens = underTest.Tokenize(new TextStream("text"));

            Assert.That(tokens.Length, Is.EqualTo(2));

            Assert.That(tokens.Peek().TokenType, Is.EqualTo(Tokens.Text));

            Assert.That(tokens.Peek().Value, Is.EqualTo("text"));

            tokens.Next();

            Assert.That(tokens.Peek().TokenType, Is.EqualTo(Tokens.EndOfLine));

            Assert.That(tokens.Peek().Value, Is.EqualTo(null));
        }

        [Test]
        public void Tokenizer_Tokenize_String()
        {
            Basic.Tokenizer.Tokenizer underTest = new Basic.Tokenizer.Tokenizer();

            ITokenCollection tokens = underTest.Tokenize(new TextStream("\"text\""));

            Assert.That(tokens.Length, Is.EqualTo(2));

            Assert.That(tokens.Peek().TokenType, Is.EqualTo(Tokens.String));

            Assert.That(tokens.Peek().Value, Is.EqualTo("text"));

            tokens.Next();

            Assert.That(tokens.Peek().TokenType, Is.EqualTo(Tokens.EndOfLine));

            Assert.That(tokens.Peek().Value, Is.EqualTo(null));
        }

        [Test]
        public void Tokenizer_Tokenize_Number()
        {
            Basic.Tokenizer.Tokenizer underTest = new Basic.Tokenizer.Tokenizer();

            ITokenCollection tokens = underTest.Tokenize(new TextStream("1234"));

            Assert.That(tokens.Length, Is.EqualTo(2));

            Assert.That(tokens.Peek().TokenType, Is.EqualTo(Tokens.Number));

            Assert.That(tokens.Peek().Value, Is.EqualTo("1234"));

            tokens.Next();

            Assert.That(tokens.Peek().TokenType, Is.EqualTo(Tokens.EndOfLine));

            Assert.That(tokens.Peek().Value, Is.EqualTo(null));
        }

        [Test]
        public void Tokenizer_Tokenize_WhiteSpace_Space()
        {
            Basic.Tokenizer.Tokenizer underTest = new Basic.Tokenizer.Tokenizer();

            ITokenCollection tokens = underTest.Tokenize(new TextStream(" "));

            Assert.That(tokens.Length, Is.EqualTo(2));

            Assert.That(tokens.Peek().TokenType, Is.EqualTo(Tokens.WhiteSpace));

            Assert.That(tokens.Peek().Value, Is.EqualTo(" "));

            tokens.Next();

            Assert.That(tokens.Peek().TokenType, Is.EqualTo(Tokens.EndOfLine));

            Assert.That(tokens.Peek().Value, Is.EqualTo(null));
        }

        [Test]
        public void Tokenizer_Tokenize_Operator_Add()
        {
            Basic.Tokenizer.Tokenizer underTest = new Basic.Tokenizer.Tokenizer();

            ITokenCollection tokens = underTest.Tokenize(new TextStream("+"));

            Assert.That(tokens.Length, Is.EqualTo(2));

            Assert.That(tokens.Peek().TokenType, Is.EqualTo(Tokens.Operator));

            Assert.That(tokens.Peek().Value, Is.EqualTo("+"));

            tokens.Next();

            Assert.That(tokens.Peek().TokenType, Is.EqualTo(Tokens.EndOfLine));

            Assert.That(tokens.Peek().Value, Is.EqualTo(null));
        }
    }
}
