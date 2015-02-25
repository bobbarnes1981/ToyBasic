using Basic.Expressions;
using Moq;
using NUnit.Framework;

namespace Basic.UnitTests
{
    [TestFixture]
    public class ParserTests
    {
        [Test]
        public void Parser_ReadExpressionNode_ParsesString()
        {
            string expectedString = "this is a string";
            ITextStream input = new TextStream(string.Format("\"{0}\"", expectedString));

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            Parser underTest = new Parser();
            IExpression expression = underTest.ReadExpressionNode(interpreterMock.Object, input, null);

            Assert.That(expression, Is.TypeOf<String>());
            Assert.That(expression.Text, Is.EqualTo(expectedString));
            Assert.That(expression.Result(), Is.EqualTo(expectedString));

            Assert.That(expression.Left, Is.Null);
            Assert.That(expression.Right, Is.Null);
        }
        
        [Test]
        public void Parser_ReadExpressionNode_ParsesNumber()
        {
            int expectedNumber = 123;
            ITextStream input = new TextStream(expectedNumber.ToString());

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            Parser underTest = new Parser();
            IExpression expression = underTest.ReadExpressionNode(interpreterMock.Object, input, null);

            Assert.That(expression, Is.TypeOf<Number>());
            Assert.That(expression.Text, Is.EqualTo(expectedNumber.ToString()));
            Assert.That(expression.Result(), Is.EqualTo(expectedNumber));

            Assert.That(expression.Left, Is.Null);
            Assert.That(expression.Right, Is.Null);
        }
        
        [Test]
        public void Parser_ReadExpressionNode_ParsesVariable()
        {
            string expectedName = "variable";
            string expectedVariable = string.Format("${0}", expectedName);
            string expectedValue = "a variable value";
            ITextStream input = new TextStream(expectedVariable);

            Mock<IHeap> heapMock = new Mock<IHeap>();
            heapMock.Setup(x => x.Get(expectedName)).Returns(expectedValue);

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();
            interpreterMock.Setup(x => x.Heap).Returns(heapMock.Object);

            Parser underTest = new Parser();
            IExpression expression = underTest.ReadExpressionNode(interpreterMock.Object, input, null);

            Assert.That(expression, Is.TypeOf<Variable>());
            Assert.That(expression.Text, Is.EqualTo(expectedName));
            Assert.That(expression.Result(), Is.EqualTo(expectedValue));

            Assert.That(expression.Left, Is.Null);
            Assert.That(expression.Right, Is.Null);
        }
        
        [Test]
        public void Parser_ReadExpressionNode_ParsesDevision()
        {
            string expectedText = "6 / 2";
            ITextStream input = new TextStream(expectedText);

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            Parser underTest = new Parser();
            IExpression expression = underTest.ReadExpressionNode(interpreterMock.Object, input, null);

            Assert.That(expression, Is.TypeOf<Operator>());
            Assert.That(expression.Text, Is.EqualTo(expectedText));

            Assert.That(expression.Left, Is.Not.Null);
            Assert.That(expression.Left, Is.TypeOf<Number>());
            Assert.That(expression.Left.Result(), Is.EqualTo(6));

            Assert.That(expression.Right, Is.Not.Null);
            Assert.That(expression.Right, Is.TypeOf<Number>());
            Assert.That(expression.Right.Result(), Is.EqualTo(2));

            Assert.That(expression.Result(), Is.EqualTo(3));
        }
        
        [Test]
        public void Parser_ReadExpressionNode_ParsesMultiplication()
        {
            string expectedText = "4 * 2";
            ITextStream input = new TextStream(expectedText);

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            Parser underTest = new Parser();
            IExpression expression = underTest.ReadExpressionNode(interpreterMock.Object, input, null);

            Assert.That(expression, Is.TypeOf<Operator>());
            Assert.That(expression.Text, Is.EqualTo(expectedText));

            Assert.That(expression.Left, Is.Not.Null);
            Assert.That(expression.Left, Is.TypeOf<Number>());
            Assert.That(expression.Left.Result(), Is.EqualTo(4));

            Assert.That(expression.Right, Is.Not.Null);
            Assert.That(expression.Right, Is.TypeOf<Number>());
            Assert.That(expression.Right.Result(), Is.EqualTo(2));

            Assert.That(expression.Result(), Is.EqualTo(8));
        }
        
        [Test]
        public void Parser_ReadExpressionNode_Addition()
        {
            string expectedText = "3 + 2";
            ITextStream input = new TextStream(expectedText);

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            Parser underTest = new Parser();
            IExpression expression = underTest.ReadExpressionNode(interpreterMock.Object, input, null);

            Assert.That(expression, Is.TypeOf<Operator>());
            Assert.That(expression.Text, Is.EqualTo(expectedText));

            Assert.That(expression.Left, Is.Not.Null);
            Assert.That(expression.Left, Is.TypeOf<Number>());
            Assert.That(expression.Left.Result(), Is.EqualTo(3));

            Assert.That(expression.Right, Is.Not.Null);
            Assert.That(expression.Right, Is.TypeOf<Number>());
            Assert.That(expression.Right.Result(), Is.EqualTo(2));

            Assert.That(expression.Result(), Is.EqualTo(5));
        }
        
        [Test]
        public void Parser_ReadExpressionNode_Subtraction()
        {
            string expectedText = "3 - 2";
            ITextStream input = new TextStream(expectedText);

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            Parser underTest = new Parser();
            IExpression expression = underTest.ReadExpressionNode(interpreterMock.Object, input, null);

            Assert.That(expression, Is.TypeOf<Operator>());
            Assert.That(expression.Text, Is.EqualTo(expectedText));

            Assert.That(expression.Left, Is.Not.Null);
            Assert.That(expression.Left, Is.TypeOf<Number>());
            Assert.That(expression.Left.Result(), Is.EqualTo(3));

            Assert.That(expression.Right, Is.Not.Null);
            Assert.That(expression.Right, Is.TypeOf<Number>());
            Assert.That(expression.Right.Result(), Is.EqualTo(2));

            Assert.That(expression.Result(), Is.EqualTo(1));
        }
        
        [Test]
        public void Parser_ReadExpressionNode_BODMAS()
        {
            ITextStream input = new TextStream("11 - 2 + 4 * 8 / 2");

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            Parser underTest = new Parser();
            IExpression expression = underTest.ReadExpressionNode(interpreterMock.Object, input, null);

            // TODO
        }
    }
}
