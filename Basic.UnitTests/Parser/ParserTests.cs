using Basic.Expressions;
using Basic.Tokenizer;
using Basic.Parser;
using Moq;
using NUnit.Framework;

namespace Basic.UnitTests.Parser
{
    [TestFixture]
    public class ParserTests
    {
        [Test]
        public void Parser_ReadExpressionNode_ParsesString()
        {
            string expectedString = "this is a string";
            string expectedText = string.Format("\"{0}\"", expectedString);
            ITokenCollection input = new TokenCollection();
            input.Add(new Token(Tokens.String, expectedString));

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            Basic.Parser.Parser underTest = new Basic.Parser.Parser();
            INode expression = underTest.ReadExpressionNode(input, null);

            Assert.That(expression, Is.TypeOf<Value>());
            Assert.That(expression.Text, Is.EqualTo(expectedText));
            Assert.That(expression.Result(interpreterMock.Object), Is.EqualTo(expectedString));

            Assert.That(expression.Left, Is.Null);
            Assert.That(expression.Right, Is.Null);
        }
        
        [Test]
        public void Parser_ReadExpressionNode_ParsesNumber()
        {
            int expectedNumber = 123;
            ITokenCollection input = new TokenCollection();
            input.Add(new Token(Tokens.Number, expectedNumber.ToString()));

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            Basic.Parser.Parser underTest = new Basic.Parser.Parser();
            INode expression = underTest.ReadExpressionNode(input, null);

            Assert.That(expression, Is.TypeOf<Value>());
            Assert.That(expression.Text, Is.EqualTo(expectedNumber.ToString()));
            Assert.That(expression.Result(interpreterMock.Object), Is.EqualTo(expectedNumber));

            Assert.That(expression.Left, Is.Null);
            Assert.That(expression.Right, Is.Null);
        }
        
        [Test]
        public void Parser_ReadExpressionNode_ParsesVariable()
        {
            string expectedName = "variable";
            string expectedVariable = string.Format("{0}$", expectedName);
            string expectedValue = "a variable value";
            ITokenCollection input = new TokenCollection();
            input.Add(new Token(Tokens.Text, expectedName));
            input.Add(new Token(Tokens.VarSufix, "$"));

            Mock<IHeap> heapMock = new Mock<IHeap>();
            heapMock.Setup(x => x.Get(expectedName)).Returns(expectedValue);

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();
            interpreterMock.Setup(x => x.Heap).Returns(heapMock.Object);

            Basic.Parser.Parser underTest = new Basic.Parser.Parser();
            INode expression = underTest.ReadExpressionNode(input, null);

            Assert.That(expression, Is.TypeOf<Value>());
            Assert.That(expression.Text, Is.EqualTo(expectedVariable));
            Assert.That(expression.Result(interpreterMock.Object), Is.EqualTo(expectedValue));

            Assert.That(expression.Left, Is.Null);
            Assert.That(expression.Right, Is.Null);
        }
        
        [Test]
        public void Parser_ReadExpressionNode_ParsesDevision()
        {
            ITokenCollection input = new TokenCollection();
            input.Add(new Token(Tokens.Number, "6"));
            input.Add(new Token(Tokens.Operator, "/"));
            input.Add(new Token(Tokens.Number, "2"));
         
            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            Basic.Parser.Parser underTest = new Basic.Parser.Parser();
            INode expression = underTest.ReadExpressionNode(input, null);

            Assert.That(expression, Is.TypeOf<Operator>());
            Assert.That(expression.Text, Is.EqualTo("6 / 2"));

            Assert.That(expression.Left, Is.Not.Null);
            Assert.That(expression.Left, Is.TypeOf<Value>());
            Assert.That(expression.Left.Result(interpreterMock.Object), Is.EqualTo(6));

            Assert.That(expression.Right, Is.Not.Null);
            Assert.That(expression.Right, Is.TypeOf<Value>());
            Assert.That(expression.Right.Result(interpreterMock.Object), Is.EqualTo(2));

            Assert.That(expression.Result(interpreterMock.Object), Is.EqualTo(3));
        }
        
        [Test]
        public void Parser_ReadExpressionNode_ParsesMultiplication()
        {
            ITokenCollection input = new TokenCollection();
            input.Add(new Token(Tokens.Number, "4"));
            input.Add(new Token(Tokens.Operator, "*"));
            input.Add(new Token(Tokens.Number, "2"));

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            Basic.Parser.Parser underTest = new Basic.Parser.Parser();
            INode expression = underTest.ReadExpressionNode(input, null);

            Assert.That(expression, Is.TypeOf<Operator>());
            Assert.That(expression.Text, Is.EqualTo("4 * 2"));

            Assert.That(expression.Left, Is.Not.Null);
            Assert.That(expression.Left, Is.TypeOf<Value>());
            Assert.That(expression.Left.Result(interpreterMock.Object), Is.EqualTo(4));
            Assert.That(expression.Left.Text, Is.EqualTo("4"));

            Assert.That(expression.Right, Is.Not.Null);
            Assert.That(expression.Right, Is.TypeOf<Value>());
            Assert.That(expression.Right.Result(interpreterMock.Object), Is.EqualTo(2));
            Assert.That(expression.Right.Text, Is.EqualTo("2"));

            Assert.That(expression.Result(interpreterMock.Object), Is.EqualTo(8));
        }
        
        [Test]
        public void Parser_ReadExpressionNode_Addition()
        {
            ITokenCollection input = new TokenCollection();
            input.Add(new Token(Tokens.Number, "3"));
            input.Add(new Token(Tokens.Operator, "+"));
            input.Add(new Token(Tokens.Number, "2"));

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            Basic.Parser.Parser underTest = new Basic.Parser.Parser();
            INode expression = underTest.ReadExpressionNode(input, null);

            Assert.That(expression, Is.TypeOf<Operator>());
            Assert.That(expression.Text, Is.EqualTo("3 + 2"));

            Assert.That(expression.Left, Is.Not.Null);
            Assert.That(expression.Left, Is.TypeOf<Value>());
            Assert.That(expression.Left.Result(interpreterMock.Object), Is.EqualTo(3));

            Assert.That(expression.Right, Is.Not.Null);
            Assert.That(expression.Right, Is.TypeOf<Value>());
            Assert.That(expression.Right.Result(interpreterMock.Object), Is.EqualTo(2));

            Assert.That(expression.Result(interpreterMock.Object), Is.EqualTo(5));
        }

        [Test]
        public void Parser_ReadExpressionNode_Subtraction()
        {
            ITokenCollection input = new TokenCollection();
            input.Add(new Token(Tokens.Number, "3"));
            input.Add(new Token(Tokens.Operator, "-"));
            input.Add(new Token(Tokens.Number, "2"));

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            Basic.Parser.Parser underTest = new Basic.Parser.Parser();
            INode expression = underTest.ReadExpressionNode(input, null);

            Assert.That(expression, Is.TypeOf<Operator>());
            Assert.That(expression.Text, Is.EqualTo("3 - 2"));

            Assert.That(expression.Left, Is.Not.Null);
            Assert.That(expression.Left, Is.TypeOf<Value>());
            Assert.That(expression.Left.Result(interpreterMock.Object), Is.EqualTo(3));
            Assert.That(expression.Left.Text, Is.EqualTo("3"));

            Assert.That(expression.Right, Is.Not.Null);
            Assert.That(expression.Right, Is.TypeOf<Value>());
            Assert.That(expression.Right.Result(interpreterMock.Object), Is.EqualTo(2));
            Assert.That(expression.Right.Text, Is.EqualTo("2"));

            Assert.That(expression.Result(interpreterMock.Object), Is.EqualTo(1));
        }

        [Test]
        public void Parser_ReadExpressionNode_PrecedenceEquality()
        {
            ITokenCollection input = new TokenCollection();
            input.Add(new Token(Tokens.Number, "8"));
            input.Add(new Token(Tokens.Operator, "=="));
            input.Add(new Token(Tokens.Number, "3"));
            input.Add(new Token(Tokens.Operator, "+"));
            input.Add(new Token(Tokens.Number, "5"));

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            Basic.Parser.Parser underTest = new Basic.Parser.Parser();
            INode expression = underTest.ReadExpressionNode(input, null);

            Assert.That(expression, Is.TypeOf<Operator>());
            Assert.That(expression.Text, Is.EqualTo("8 == 3 + 5"));

            Assert.That(expression.Result(interpreterMock.Object), Is.EqualTo(true));
        }
        
        [Test]
        public void Parser_ReadExpressionNode_BODMAS_Reversed()
        {
            ITokenCollection input = new TokenCollection();
            input.Add(new Token(Tokens.Number, "11"));
            input.Add(new Token(Tokens.Operator, "-"));
            input.Add(new Token(Tokens.Number, "2"));
            input.Add(new Token(Tokens.Operator, "+"));
            input.Add(new Token(Tokens.Number, "4"));
            input.Add(new Token(Tokens.Operator, "*"));
            input.Add(new Token(Tokens.Number, "4"));
            input.Add(new Token(Tokens.Operator, "/"));
            input.Add(new Token(Tokens.Number, "2"));

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            Basic.Parser.Parser underTest = new Basic.Parser.Parser();
            INode expression = underTest.ReadExpressionNode(input, null);

            // 1 top level
            Assert.That(expression, Is.TypeOf<Operator>());
            Assert.That(expression.Text, Is.EqualTo("11 - 2 + 4 * 4 / 2"));
            Assert.That(expression.Result(interpreterMock.Object), Is.EqualTo(1));
            Assert.That(expression.Left, Is.Not.Null);
            Assert.That(expression.Right, Is.Not.Null);

            // 2 left node
            Assert.That(expression.Left, Is.TypeOf<Value>());
            Assert.That(expression.Left.Text, Is.EqualTo("11"));
            Assert.That(expression.Left.Result(interpreterMock.Object), Is.EqualTo(11));
            Assert.That(expression.Left.Left, Is.Null);
            Assert.That(expression.Left.Right, Is.Null);

            // 2 right node
            Assert.That(expression.Right, Is.TypeOf<Operator>());
            Assert.That(expression.Right.Text, Is.EqualTo("2 + 4 * 4 / 2"));
            Assert.That(expression.Right.Result(interpreterMock.Object), Is.EqualTo(10));
            Assert.That(expression.Right.Left, Is.Not.Null);
            Assert.That(expression.Right.Right, Is.Not.Null);

            // 3 left node
            Assert.That(expression.Right.Left, Is.TypeOf<Value>());
            Assert.That(expression.Right.Left.Text, Is.EqualTo("2"));
            Assert.That(expression.Right.Left.Result(interpreterMock.Object), Is.EqualTo(2));
            Assert.That(expression.Right.Left.Left, Is.Null);
            Assert.That(expression.Right.Left.Right, Is.Null);

            // 3 right node
            Assert.That(expression.Right.Right, Is.TypeOf<Operator>());
            Assert.That(expression.Right.Right.Text, Is.EqualTo("4 * 4 / 2"));
            Assert.That(expression.Right.Right.Result(interpreterMock.Object), Is.EqualTo(8));
            Assert.That(expression.Right.Right.Left, Is.Not.Null);
            Assert.That(expression.Right.Right.Right, Is.Not.Null);

            // 4 left node
            Assert.That(expression.Right.Right.Left, Is.TypeOf<Value>());
            Assert.That(expression.Right.Right.Left.Text, Is.EqualTo("4"));
            Assert.That(expression.Right.Right.Left.Result(interpreterMock.Object), Is.EqualTo(4));
            Assert.That(expression.Right.Right.Left.Left, Is.Null);
            Assert.That(expression.Right.Right.Left.Right, Is.Null);

            // 4 right node
            Assert.That(expression.Right.Right.Right, Is.TypeOf<Operator>());
            Assert.That(expression.Right.Right.Right.Text, Is.EqualTo("4 / 2"));
            Assert.That(expression.Right.Right.Right.Result(interpreterMock.Object), Is.EqualTo(2));
            Assert.That(expression.Right.Right.Right.Left, Is.Not.Null);
            Assert.That(expression.Right.Right.Right.Right, Is.Not.Null);

            // 5 left node
            Assert.That(expression.Right.Right.Right.Left, Is.TypeOf<Value>());
            Assert.That(expression.Right.Right.Right.Left.Text, Is.EqualTo("4"));
            Assert.That(expression.Right.Right.Right.Left.Result(interpreterMock.Object), Is.EqualTo(4));
            Assert.That(expression.Right.Right.Right.Left.Left, Is.Null);
            Assert.That(expression.Right.Right.Right.Left.Right, Is.Null);

            // 5 right node
            Assert.That(expression.Right.Right.Right.Right, Is.TypeOf<Value>());
            Assert.That(expression.Right.Right.Right.Right.Text, Is.EqualTo("2"));
            Assert.That(expression.Right.Right.Right.Right.Result(interpreterMock.Object), Is.EqualTo(2));
            Assert.That(expression.Right.Right.Right.Right.Left, Is.Null);
            Assert.That(expression.Right.Right.Right.Right.Right, Is.Null);
        }

        [Test]
        public void Parser_ReadExpressionNode_BODMAS_Combined()
        {
            ITokenCollection input = new TokenCollection();
            input.Add(new Token(Tokens.Number, "10"));
            input.Add(new Token(Tokens.Operator, "/"));
            input.Add(new Token(Tokens.Number, "2"));
            input.Add(new Token(Tokens.Operator, "+"));
            input.Add(new Token(Tokens.Number, "4"));
            input.Add(new Token(Tokens.Operator, "*"));
            input.Add(new Token(Tokens.Number, "4"));
            input.Add(new Token(Tokens.Operator, "/"));
            input.Add(new Token(Tokens.Number, "2"));

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            Basic.Parser.Parser underTest = new Basic.Parser.Parser();
            INode expression = underTest.ReadExpressionNode(input, null);

            // 1 top level
            Assert.That(expression, Is.TypeOf<Operator>());
            Assert.That(expression.Text, Is.EqualTo("10 / 2 + 4 * 4 / 2"));
            Assert.That(expression.Result(interpreterMock.Object), Is.EqualTo(13));
            Assert.That(expression.Left, Is.Not.Null);
            Assert.That(expression.Right, Is.Not.Null);

            // 2 left 
            Assert.That(expression.Left, Is.TypeOf<Operator>());
            Assert.That(expression.Left.Text, Is.EqualTo("10 / 2"));
            Assert.That(expression.Left.Result(interpreterMock.Object), Is.EqualTo(5));
            Assert.That(expression.Left.Left, Is.Not.Null);
            Assert.That(expression.Left.Right, Is.Not.Null);

            // 3 left left
            Assert.That(expression.Left.Left, Is.TypeOf<Value>());
            Assert.That(expression.Left.Left.Text, Is.EqualTo("10"));
            Assert.That(expression.Left.Left.Result(interpreterMock.Object), Is.EqualTo(10));
            Assert.That(expression.Left.Left.Left, Is.Null);
            Assert.That(expression.Left.Left.Right, Is.Null);

            // 3 left right
            Assert.That(expression.Left.Right, Is.TypeOf<Value>());
            Assert.That(expression.Left.Right.Text, Is.EqualTo("2"));
            Assert.That(expression.Left.Right.Result(interpreterMock.Object), Is.EqualTo(2));
            Assert.That(expression.Left.Right.Left, Is.Null);
            Assert.That(expression.Left.Right.Right, Is.Null);

            // 2 right 
            Assert.That(expression.Right, Is.TypeOf<Operator>());
            Assert.That(expression.Right.Text, Is.EqualTo("4 * 4 / 2"));
            Assert.That(expression.Right.Result(interpreterMock.Object), Is.EqualTo(8));
            Assert.That(expression.Right.Left, Is.Not.Null);
            Assert.That(expression.Right.Right, Is.Not.Null);

            // 3 right left
            Assert.That(expression.Right.Left, Is.TypeOf<Value>());
            Assert.That(expression.Right.Left.Text, Is.EqualTo("4"));
            Assert.That(expression.Right.Left.Result(interpreterMock.Object), Is.EqualTo(4));
            Assert.That(expression.Right.Left.Left, Is.Null);
            Assert.That(expression.Right.Left.Right, Is.Null);

            // 3 right right
            Assert.That(expression.Right.Right, Is.TypeOf<Operator>());
            Assert.That(expression.Right.Right.Text, Is.EqualTo("4 / 2"));
            Assert.That(expression.Right.Right.Result(interpreterMock.Object), Is.EqualTo(2));
            Assert.That(expression.Right.Right.Left, Is.Not.Null);
            Assert.That(expression.Right.Right.Right, Is.Not.Null);

            // 4 right right left
            Assert.That(expression.Right.Right.Left, Is.TypeOf<Value>());
            Assert.That(expression.Right.Right.Left.Text, Is.EqualTo("4"));
            Assert.That(expression.Right.Right.Left.Result(interpreterMock.Object), Is.EqualTo(4));
            Assert.That(expression.Right.Right.Left.Left, Is.Null);
            Assert.That(expression.Right.Right.Left.Right, Is.Null);

            // 4 right right right
            Assert.That(expression.Right.Right.Right, Is.TypeOf<Value>());
            Assert.That(expression.Right.Right.Right.Text, Is.EqualTo("2"));
            Assert.That(expression.Right.Right.Right.Result(interpreterMock.Object), Is.EqualTo(2));
            Assert.That(expression.Right.Right.Right.Left, Is.Null);
            Assert.That(expression.Right.Right.Right.Right, Is.Null);
        }

        [Test]
        public void Parser_ReadExpressionNode_NoBrackets()
        {
            ITokenCollection input = new TokenCollection();
            input.Add(new Token(Tokens.Number, "5"));
            input.Add(new Token(Tokens.Operator, "+"));
            input.Add(new Token(Tokens.Number, "5"));
            input.Add(new Token(Tokens.Operator, "/"));
            input.Add(new Token(Tokens.Number, "2"));

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            Basic.Parser.Parser underTest = new Basic.Parser.Parser();
            INode expression = underTest.ReadExpressionNode(input, null);

            // 1 top level
            Assert.That(expression, Is.TypeOf<Operator>());
            Assert.That(expression.Text, Is.EqualTo("5 + 5 / 2"));
            Assert.That(expression.Result(interpreterMock.Object), Is.EqualTo(7));
            Assert.That(expression.Left, Is.Not.Null);
            Assert.That(expression.Right, Is.Not.Null);

            // 2 left
            Assert.That(expression.Left, Is.TypeOf<Value>());
            Assert.That(expression.Left.Text, Is.EqualTo("5"));
            Assert.That(expression.Left.Result(interpreterMock.Object), Is.EqualTo(5));
            Assert.That(expression.Left.Left, Is.Null);
            Assert.That(expression.Left.Right, Is.Null);

            // 2 right
            Assert.That(expression.Right, Is.TypeOf<Operator>());
            Assert.That(expression.Right.Text, Is.EqualTo("5 / 2"));
            Assert.That(expression.Right.Result(interpreterMock.Object), Is.EqualTo(2));
            Assert.That(expression.Right.Left, Is.Not.Null);
            Assert.That(expression.Right.Right, Is.Not.Null);

            // 3 right left
            Assert.That(expression.Right.Left, Is.TypeOf<Value>());
            Assert.That(expression.Right.Left.Text, Is.EqualTo("5"));
            Assert.That(expression.Right.Left.Result(interpreterMock.Object), Is.EqualTo(5));
            Assert.That(expression.Right.Left.Left, Is.Null);
            Assert.That(expression.Right.Left.Right, Is.Null);

            // 3 right right
            Assert.That(expression.Right.Right, Is.TypeOf<Value>());
            Assert.That(expression.Right.Right.Text, Is.EqualTo("2"));
            Assert.That(expression.Right.Right.Result(interpreterMock.Object), Is.EqualTo(2));
            Assert.That(expression.Right.Right.Left, Is.Null);
            Assert.That(expression.Right.Right.Right, Is.Null);
        }

        [Test]
        public void Parser_ReadExpressionNode_Brackets()
        {
            ITokenCollection input = new TokenCollection();
            input.Add(new Token(Tokens.Bracket, "("));
            input.Add(new Token(Tokens.Number, "5"));
            input.Add(new Token(Tokens.Operator, "+"));
            input.Add(new Token(Tokens.Number, "5"));
            input.Add(new Token(Tokens.Bracket, ")"));
            input.Add(new Token(Tokens.Operator, "/"));
            input.Add(new Token(Tokens.Number, "2"));

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            Basic.Parser.Parser underTest = new Basic.Parser.Parser();
            INode expression = underTest.ReadExpressionNode(input, null);

            // 1 top level
            Assert.That(expression, Is.TypeOf<Operator>());
            Assert.That(expression.Text, Is.EqualTo("(5 + 5) / 2"));
            Assert.That(expression.Result(interpreterMock.Object), Is.EqualTo(5));
            Assert.That(expression.Left, Is.Not.Null);
            Assert.That(expression.Right, Is.Not.Null);

            // 2 left
            Assert.That(expression.Left, Is.TypeOf<Brackets>());
            Assert.That(expression.Left.Text, Is.EqualTo("(5 + 5)"));
            Assert.That(expression.Left.Result(interpreterMock.Object), Is.EqualTo(10));
            Assert.That(expression.Left.Left, Is.Not.Null);
            Assert.That(expression.Left.Right, Is.Null);

            // 2 right
            Assert.That(expression.Right, Is.TypeOf<Value>());
            Assert.That(expression.Right.Text, Is.EqualTo("2"));
            Assert.That(expression.Right.Result(interpreterMock.Object), Is.EqualTo(2));
            Assert.That(expression.Right.Left, Is.Null);
            Assert.That(expression.Right.Right, Is.Null);

            // 3 left left left
            Assert.That(expression.Left.Left, Is.TypeOf<Operator>());
            Assert.That(expression.Left.Left.Text, Is.EqualTo("5 + 5"));
            Assert.That(expression.Left.Left.Result(interpreterMock.Object), Is.EqualTo(10));
            Assert.That(expression.Left.Left.Left, Is.Not.Null);
            Assert.That(expression.Left.Left.Right, Is.Not.Null);

            // 4 left left left
            Assert.That(expression.Left.Left.Left, Is.TypeOf<Value>());
            Assert.That(expression.Left.Left.Left.Text, Is.EqualTo("5"));
            Assert.That(expression.Left.Left.Left.Result(interpreterMock.Object), Is.EqualTo(5));
            Assert.That(expression.Left.Left.Left.Left, Is.Null);
            Assert.That(expression.Left.Left.Left.Right, Is.Null);

            // 4 left left right
            Assert.That(expression.Left.Left.Right, Is.TypeOf<Value>());
            Assert.That(expression.Left.Left.Right.Text, Is.EqualTo("5"));
            Assert.That(expression.Left.Left.Right.Result(interpreterMock.Object), Is.EqualTo(5));
            Assert.That(expression.Left.Left.Right.Left, Is.Null);
            Assert.That(expression.Left.Left.Right.Right, Is.Null);
        }

        [Test]
        public void Parser_ReadExpressionNode_OperatorParsing()
        {
            ITokenCollection input = new TokenCollection();
            input.Add(new Token(Tokens.Bracket, "("));
            input.Add(new Token(Tokens.Number, "5"));
            input.Add(new Token(Tokens.WhiteSpace, " "));
            input.Add(new Token(Tokens.Operator, "+"));
            input.Add(new Token(Tokens.WhiteSpace, " "));
            input.Add(new Token(Tokens.Number, "5"));
            input.Add(new Token(Tokens.WhiteSpace, " "));
            input.Add(new Token(Tokens.Bracket, ")"));
            input.Add(new Token(Tokens.Operator,"/" ));
            input.Add(new Token(Tokens.WhiteSpace, " "));
            input.Add(new Token(Tokens.Number, "2"));

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            Basic.Parser.Parser underTest = new Basic.Parser.Parser();
            INode expression = underTest.ReadExpressionNode(input, null);

            // 1 top level
            Assert.That(expression, Is.TypeOf<Operator>());
            Assert.That(expression.Text, Is.EqualTo("(5 + 5) / 2"));
            Assert.That(expression.Result(interpreterMock.Object), Is.EqualTo(5));
            Assert.That(expression.Left, Is.Not.Null);
            Assert.That(expression.Right, Is.Not.Null);

            // 2 left
            Assert.That(expression.Left, Is.TypeOf<Brackets>());
            Assert.That(expression.Left.Text, Is.EqualTo("(5 + 5)"));
            Assert.That(expression.Left.Result(interpreterMock.Object), Is.EqualTo(10));
            Assert.That(expression.Left.Left, Is.Not.Null);
            Assert.That(expression.Left.Right, Is.Null);

            // 2 right
            Assert.That(expression.Right, Is.TypeOf<Value>());
            Assert.That(expression.Right.Text, Is.EqualTo("2"));
            Assert.That(expression.Right.Result(interpreterMock.Object), Is.EqualTo(2));
            Assert.That(expression.Right.Left, Is.Null);
            Assert.That(expression.Right.Right, Is.Null);

            // 3 left left left
            Assert.That(expression.Left.Left, Is.TypeOf<Operator>());
            Assert.That(expression.Left.Left.Text, Is.EqualTo("5 + 5"));
            Assert.That(expression.Left.Left.Result(interpreterMock.Object), Is.EqualTo(10));
            Assert.That(expression.Left.Left.Left, Is.Not.Null);
            Assert.That(expression.Left.Left.Right, Is.Not.Null);

            // 4 left left left
            Assert.That(expression.Left.Left.Left, Is.TypeOf<Value>());
            Assert.That(expression.Left.Left.Left.Text, Is.EqualTo("5"));
            Assert.That(expression.Left.Left.Left.Result(interpreterMock.Object), Is.EqualTo(5));
            Assert.That(expression.Left.Left.Left.Left, Is.Null);
            Assert.That(expression.Left.Left.Left.Right, Is.Null);

            // 4 left left right
            Assert.That(expression.Left.Left.Right, Is.TypeOf<Value>());
            Assert.That(expression.Left.Left.Right.Text, Is.EqualTo("5"));
            Assert.That(expression.Left.Left.Right.Result(interpreterMock.Object), Is.EqualTo(5));
            Assert.That(expression.Left.Left.Right.Left, Is.Null);
            Assert.That(expression.Left.Left.Right.Right, Is.Null);
        }

        [Test]
        public void Parser_ReadExpressionNode_PointlessBrackets()
        {
            ITokenCollection input = new TokenCollection();
            input.Add(new Token(Tokens.Bracket, "("));
            input.Add(new Token(Tokens.Number, "5"));
            input.Add(new Token(Tokens.Bracket, ")"));
            input.Add(new Token(Tokens.Operator, "+"));
            input.Add(new Token(Tokens.Number, "2"));

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            Basic.Parser.Parser underTest = new Basic.Parser.Parser();
            INode expression = underTest.ReadExpressionNode(input, null);

            // 1 top level
            Assert.That(expression, Is.TypeOf<Operator>());
            Assert.That(expression.Text, Is.EqualTo("(5) + 2"));
            Assert.That(expression.Result(interpreterMock.Object), Is.EqualTo(7));
            Assert.That(expression.Left, Is.Not.Null);
            Assert.That(expression.Right, Is.Not.Null);

            // 2 left
            Assert.That(expression.Left, Is.TypeOf<Brackets>());
            Assert.That(expression.Left.Text, Is.EqualTo("(5)"));
            Assert.That(expression.Left.Result(interpreterMock.Object), Is.EqualTo(5));
            Assert.That(expression.Left.Left, Is.Not.Null);
            Assert.That(expression.Left.Right, Is.Null);

            // 2 right
            Assert.That(expression.Right, Is.TypeOf<Value>());
            Assert.That(expression.Right.Text, Is.EqualTo("2"));
            Assert.That(expression.Right.Result(interpreterMock.Object), Is.EqualTo(2));
            Assert.That(expression.Right.Left, Is.Null);
            Assert.That(expression.Right.Right, Is.Null);

            // 3 left left
            Assert.That(expression.Left.Left, Is.TypeOf<Value>());
            Assert.That(expression.Left.Left.Text, Is.EqualTo("5"));
            Assert.That(expression.Left.Left.Result(interpreterMock.Object), Is.EqualTo(5));
            Assert.That(expression.Left.Left.Left, Is.Null);
            Assert.That(expression.Left.Left.Right, Is.Null);
        }

        [Test]
        public void Parser_ReadExpressionNode_TrailingBrackets()
        {
            ITokenCollection input = new TokenCollection();
            input.Add(new Token(Tokens.Bracket, "("));
            input.Add(new Token(Tokens.Number, "5"));
            input.Add(new Token(Tokens.Operator, "+"));
            input.Add(new Token(Tokens.Number, "2"));
            input.Add(new Token(Tokens.Bracket, ")"));

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            Basic.Parser.Parser underTest = new Basic.Parser.Parser();
            INode expression = underTest.ReadExpressionNode(input, null);

            // 1 top level
            Assert.That(expression, Is.TypeOf<Brackets>());
            Assert.That(expression.Text, Is.EqualTo("(5 + 2)"));
            Assert.That(expression.Result(interpreterMock.Object), Is.EqualTo(7));
            Assert.That(expression.Left, Is.Not.Null);
            Assert.That(expression.Right, Is.Null);

            // 2 left
            Assert.That(expression.Left, Is.TypeOf<Operator>());
            Assert.That(expression.Left.Text, Is.EqualTo("5 + 2"));
            Assert.That(expression.Left.Result(interpreterMock.Object), Is.EqualTo(7));
            Assert.That(expression.Left.Left, Is.Not.Null);
            Assert.That(expression.Left.Right, Is.Not.Null);

            // 3 left left
            Assert.That(expression.Left.Left, Is.TypeOf<Value>());
            Assert.That(expression.Left.Left.Text, Is.EqualTo("5"));
            Assert.That(expression.Left.Left.Result(interpreterMock.Object), Is.EqualTo(5));
            Assert.That(expression.Left.Left.Left, Is.Null);
            Assert.That(expression.Left.Left.Right, Is.Null);

            // 3 left right
            Assert.That(expression.Left.Right, Is.TypeOf<Value>());
            Assert.That(expression.Left.Right.Text, Is.EqualTo("2"));
            Assert.That(expression.Left.Right.Result(interpreterMock.Object), Is.EqualTo(2));
            Assert.That(expression.Left.Right.Left, Is.Null);
            Assert.That(expression.Left.Right.Right, Is.Null);
        }

        [Test]
        public void Parser_ReadExpressionNode_Not()
        {
            ITokenCollection input = new TokenCollection();
            input.Add(new Token(Tokens.Operator, "!"));
            input.Add(new Token(Tokens.Number, "8"));
            input.Add(new Token(Tokens.Operator, "=="));
            input.Add(new Token(Tokens.Number, "8"));

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            Basic.Parser.Parser underTest = new Basic.Parser.Parser();
            INode expression = underTest.ReadExpressionNode(input, null);

            // 1 top level
            Assert.That(expression, Is.TypeOf<Not>());
            Assert.That(expression.Text, Is.EqualTo("!8 == 8"));
            Assert.That(expression.Result(interpreterMock.Object), Is.EqualTo(false));
            Assert.That(expression.Left, Is.Not.Null);
            Assert.That(expression.Right, Is.Null);

            // 2 left
            Assert.That(expression.Left, Is.TypeOf<Operator>());
            Assert.That(expression.Left.Text, Is.EqualTo("8 == 8"));
            Assert.That(expression.Left.Result(interpreterMock.Object), Is.EqualTo(true));
            Assert.That(expression.Left.Left, Is.Not.Null);
            Assert.That(expression.Left.Right, Is.Not.Null);

            // 3 left left
            Assert.That(expression.Left.Left, Is.TypeOf<Value>());
            Assert.That(expression.Left.Left.Text, Is.EqualTo("8"));
            Assert.That(expression.Left.Left.Result(interpreterMock.Object), Is.EqualTo(8));
            Assert.That(expression.Left.Left.Left, Is.Null);
            Assert.That(expression.Left.Left.Right, Is.Null);

            // 3 left right
            Assert.That(expression.Left.Right, Is.TypeOf<Value>());
            Assert.That(expression.Left.Right.Text, Is.EqualTo("8"));
            Assert.That(expression.Left.Right.Result(interpreterMock.Object), Is.EqualTo(8));
            Assert.That(expression.Left.Right.Left, Is.Null);
            Assert.That(expression.Left.Right.Right, Is.Null);
        }
    }
}
