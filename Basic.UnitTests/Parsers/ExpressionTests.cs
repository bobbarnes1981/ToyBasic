﻿using Basic.Expressions;
using Moq;
using NUnit.Framework;

namespace Basic.UnitTests.Parsers
{
    [TestFixture]
    public class ExpressionTests
    {
        [Test]
        public void Expression_ReadExpressionNode_ParsesString()
        {
            string expectedString = "this is a string";
            ITextStream input = new TextStream(string.Format("\"{0}\"", expectedString));

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            Basic.Parsers.Expression underTest = new Basic.Parsers.Expression();
            IExpression expression = underTest.ReadExpressionNode(interpreterMock.Object, input, null);

            Assert.That(expression, Is.TypeOf<String>());
            Assert.That(expression.Text, Is.EqualTo(expectedString));
            Assert.That(expression.Result(), Is.EqualTo(expectedString));

            Assert.That(expression.Left, Is.Null);
            Assert.That(expression.Right, Is.Null);
        }
        
        [Test]
        public void Expression_ReadExpressionNode_ParsesNumber()
        {
            int expectedNumber = 123;
            ITextStream input = new TextStream(expectedNumber.ToString());

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            Basic.Parsers.Expression underTest = new Basic.Parsers.Expression();
            IExpression expression = underTest.ReadExpressionNode(interpreterMock.Object, input, null);

            Assert.That(expression, Is.TypeOf<Number>());
            Assert.That(expression.Text, Is.EqualTo(expectedNumber.ToString()));
            Assert.That(expression.Result(), Is.EqualTo(expectedNumber));

            Assert.That(expression.Left, Is.Null);
            Assert.That(expression.Right, Is.Null);
        }
        
        [Test]
        public void Expression_ReadExpressionNode_ParsesVariable()
        {
            string expectedName = "variable";
            string expectedVariable = string.Format("${0}", expectedName);
            string expectedValue = "a variable value";
            ITextStream input = new TextStream(expectedVariable);

            Mock<IHeap> heapMock = new Mock<IHeap>();
            heapMock.Setup(x => x.Get(expectedName)).Returns(expectedValue);

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();
            interpreterMock.Setup(x => x.Heap).Returns(heapMock.Object);

            Basic.Parsers.Expression underTest = new Basic.Parsers.Expression();
            IExpression expression = underTest.ReadExpressionNode(interpreterMock.Object, input, null);

            Assert.That(expression, Is.TypeOf<Variable>());
            Assert.That(expression.Text, Is.EqualTo(expectedName));
            Assert.That(expression.Result(), Is.EqualTo(expectedValue));

            Assert.That(expression.Left, Is.Null);
            Assert.That(expression.Right, Is.Null);
        }
        
        [Test]
        public void Expression_ReadExpressionNode_ParsesDevision()
        {
            string expectedText = "6 / 2";
            ITextStream input = new TextStream(expectedText);

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            Basic.Parsers.Expression underTest = new Basic.Parsers.Expression();
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
        public void Expression_ReadExpressionNode_ParsesMultiplication()
        {
            string expectedText = "4 * 2";
            ITextStream input = new TextStream(expectedText);

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            Basic.Parsers.Expression underTest = new Basic.Parsers.Expression();
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
        public void Expression_ReadExpressionNode_Addition()
        {
            string expectedText = "3 + 2";
            ITextStream input = new TextStream(expectedText);

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            Basic.Parsers.Expression underTest = new Basic.Parsers.Expression();
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
        public void Expression_ReadExpressionNode_Subtraction()
        {
            string expectedText = "3 - 2";
            ITextStream input = new TextStream(expectedText);

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            Basic.Parsers.Expression underTest = new Basic.Parsers.Expression();
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
        public void Expression_ReadExpressionNode_BODMAS_Reversed()
        {
            string expectedText = "11 - 2 + 4 * 4 / 2";
            ITextStream input = new TextStream(expectedText);

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            Basic.Parsers.Expression underTest = new Basic.Parsers.Expression();
            IExpression expression = underTest.ReadExpressionNode(interpreterMock.Object, input, null);

            // 1 top level
            Assert.That(expression, Is.TypeOf<Operator>());
            Assert.That(expression.Text, Is.EqualTo(expectedText));
            Assert.That(expression.Result(), Is.EqualTo(1));
            Assert.That(expression.Left, Is.Not.Null);
            Assert.That(expression.Right, Is.Not.Null);

            // 2 left node
            Assert.That(expression.Left, Is.TypeOf<Number>());
            Assert.That(expression.Left.Text, Is.EqualTo("11"));
            Assert.That(expression.Left.Result(), Is.EqualTo(11));
            Assert.That(expression.Left.Left, Is.Null);
            Assert.That(expression.Left.Right, Is.Null);

            // 2 right node
            Assert.That(expression.Right, Is.TypeOf<Operator>());
            Assert.That(expression.Right.Text, Is.EqualTo("2 + 4 * 4 / 2"));
            Assert.That(expression.Right.Result(), Is.EqualTo(10));
            Assert.That(expression.Right.Left, Is.Not.Null);
            Assert.That(expression.Right.Right, Is.Not.Null);

            // 3 left node
            Assert.That(expression.Right.Left, Is.TypeOf<Number>());
            Assert.That(expression.Right.Left.Text, Is.EqualTo("2"));
            Assert.That(expression.Right.Left.Result(), Is.EqualTo(2));
            Assert.That(expression.Right.Left.Left, Is.Null);
            Assert.That(expression.Right.Left.Right, Is.Null);

            // 3 right node
            Assert.That(expression.Right.Right, Is.TypeOf<Operator>());
            Assert.That(expression.Right.Right.Text, Is.EqualTo("4 * 4 / 2"));
            Assert.That(expression.Right.Right.Result(), Is.EqualTo(8));
            Assert.That(expression.Right.Right.Left, Is.Not.Null);
            Assert.That(expression.Right.Right.Right, Is.Not.Null);

            // 4 left node
            Assert.That(expression.Right.Right.Left, Is.TypeOf<Number>());
            Assert.That(expression.Right.Right.Left.Text, Is.EqualTo("4"));
            Assert.That(expression.Right.Right.Left.Result(), Is.EqualTo(4));
            Assert.That(expression.Right.Right.Left.Left, Is.Null);
            Assert.That(expression.Right.Right.Left.Right, Is.Null);

            // 4 right node
            Assert.That(expression.Right.Right.Right, Is.TypeOf<Operator>());
            Assert.That(expression.Right.Right.Right.Text, Is.EqualTo("4 / 2"));
            Assert.That(expression.Right.Right.Right.Result(), Is.EqualTo(2));
            Assert.That(expression.Right.Right.Right.Left, Is.Not.Null);
            Assert.That(expression.Right.Right.Right.Right, Is.Not.Null);

            // 5 left node
            Assert.That(expression.Right.Right.Right.Left, Is.TypeOf<Number>());
            Assert.That(expression.Right.Right.Right.Left.Text, Is.EqualTo("4"));
            Assert.That(expression.Right.Right.Right.Left.Result(), Is.EqualTo(4));
            Assert.That(expression.Right.Right.Right.Left.Left, Is.Null);
            Assert.That(expression.Right.Right.Right.Left.Right, Is.Null);

            // 5 right node
            Assert.That(expression.Right.Right.Right.Right, Is.TypeOf<Number>());
            Assert.That(expression.Right.Right.Right.Right.Text, Is.EqualTo("2"));
            Assert.That(expression.Right.Right.Right.Right.Result(), Is.EqualTo(2));
            Assert.That(expression.Right.Right.Right.Right.Left, Is.Null);
            Assert.That(expression.Right.Right.Right.Right.Right, Is.Null);

        }

        [Test]
        public void Expression_ReadExpressionNode_BODMAS_Combined()
        {
            string expectedText = "10 / 2 + 4 * 4 / 2";
            ITextStream input = new TextStream(expectedText);

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            Basic.Parsers.Expression underTest = new Basic.Parsers.Expression();
            IExpression expression = underTest.ReadExpressionNode(interpreterMock.Object, input, null);

            // 1 top level
            Assert.That(expression, Is.TypeOf<Operator>());
            Assert.That(expression.Text, Is.EqualTo(expectedText));
            Assert.That(expression.Result(), Is.EqualTo(13));
            Assert.That(expression.Left, Is.Not.Null);
            Assert.That(expression.Right, Is.Not.Null);

            // 2 left 
            Assert.That(expression.Left, Is.TypeOf<Operator>());
            Assert.That(expression.Left.Text, Is.EqualTo("10 / 2"));
            Assert.That(expression.Left.Result(), Is.EqualTo(5));
            Assert.That(expression.Left.Left, Is.Not.Null);
            Assert.That(expression.Left.Right, Is.Not.Null);

            // 3 left left
            Assert.That(expression.Left.Left, Is.TypeOf<Number>());
            Assert.That(expression.Left.Left.Text, Is.EqualTo("10"));
            Assert.That(expression.Left.Left.Result(), Is.EqualTo(10));
            Assert.That(expression.Left.Left.Left, Is.Null);
            Assert.That(expression.Left.Left.Right, Is.Null);

            // 3 left right
            Assert.That(expression.Left.Right, Is.TypeOf<Number>());
            Assert.That(expression.Left.Right.Text, Is.EqualTo("2"));
            Assert.That(expression.Left.Right.Result(), Is.EqualTo(2));
            Assert.That(expression.Left.Right.Left, Is.Null);
            Assert.That(expression.Left.Right.Right, Is.Null);

            // 2 right 
            Assert.That(expression.Right, Is.TypeOf<Operator>());
            Assert.That(expression.Right.Text, Is.EqualTo("4 * 4 / 2"));
            Assert.That(expression.Right.Result(), Is.EqualTo(8));
            Assert.That(expression.Right.Left, Is.Not.Null);
            Assert.That(expression.Right.Right, Is.Not.Null);

            // 3 right left
            Assert.That(expression.Right.Left, Is.TypeOf<Number>());
            Assert.That(expression.Right.Left.Text, Is.EqualTo("4"));
            Assert.That(expression.Right.Left.Result(), Is.EqualTo(4));
            Assert.That(expression.Right.Left.Left, Is.Null);
            Assert.That(expression.Right.Left.Right, Is.Null);

            // 3 right right
            Assert.That(expression.Right.Right, Is.TypeOf<Operator>());
            Assert.That(expression.Right.Right.Text, Is.EqualTo("4 / 2"));
            Assert.That(expression.Right.Right.Result(), Is.EqualTo(2));
            Assert.That(expression.Right.Right.Left, Is.Not.Null);
            Assert.That(expression.Right.Right.Right, Is.Not.Null);

            // 4 right right left
            Assert.That(expression.Right.Right.Left, Is.TypeOf<Number>());
            Assert.That(expression.Right.Right.Left.Text, Is.EqualTo("4"));
            Assert.That(expression.Right.Right.Left.Result(), Is.EqualTo(4));
            Assert.That(expression.Right.Right.Left.Left, Is.Null);
            Assert.That(expression.Right.Right.Left.Right, Is.Null);

            // 4 right right right
            Assert.That(expression.Right.Right.Right, Is.TypeOf<Number>());
            Assert.That(expression.Right.Right.Right.Text, Is.EqualTo("2"));
            Assert.That(expression.Right.Right.Right.Result(), Is.EqualTo(2));
            Assert.That(expression.Right.Right.Right.Left, Is.Null);
            Assert.That(expression.Right.Right.Right.Right, Is.Null);


        }
    }
}