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

            Parser underTest = new Parser();
            Expressions.IExpression expression = underTest.ReadExpressionNode(input, null);

            // TODO
        }
        
        [Test]
        public void Parser_ReadExpressionNode_ParsesNumber()
        {
            int expectedNumber = 123;
            ITextStream input = new TextStream(expectedNumber.ToString());

            Parser underTest = new Parser();
            Expressions.IExpression expression = underTest.ReadExpressionNode(input, null);

            // TODO
        }
        
        [Test]
        public void Parser_ReadExpressionNode_ParsesVariable()
        {
            string expectedVariable = "$variable";
            ITextStream input = new TextStream(expectedVariable);

            Parser underTest = new Parser();
            Expressions.IExpression expression = underTest.ReadExpressionNode(input, null);

            // TODO
        }
        
        [Test]
        public void Parser_ReadExpressionNode_ParsesDevision()
        {
            ITextStream input = new TextStream("");

            Parser underTest = new Parser();
            Expressions.IExpression expression = underTest.ReadExpressionNode(input, null);

            // TODO
        }
        
        [Test]
        public void Parser_ReadExpressionNode_ParsesMultiplication()
        {
            ITextStream input = new TextStream("");

            Parser underTest = new Parser();
            Expressions.IExpression expression = underTest.ReadExpressionNode(input, null);

            // TODO
        }
        
        [Test]
        public void Parser_ReadExpressionNode_Addition()
        {
            ITextStream input = new TextStream("");

            Parser underTest = new Parser();
            Expressions.IExpression expression = underTest.ReadExpressionNode(input, null);

            // TODO
        }
        
        [Test]
        public void Parser_ReadExpressionNode_Subtraction()
        {
            ITextStream input = new TextStream("");

            Parser underTest = new Parser();
            Expressions.IExpression expression = underTest.ReadExpressionNode(input, null);

            // TODO
        }
        
        [Test]
        public void Parser_ReadExpressionNode_BODMAS()
        {
            ITextStream input = new TextStream("");

            Parser underTest = new Parser();
            Expressions.IExpression expression = underTest.ReadExpressionNode(input, null);

            // TODO
        }
    }
}
