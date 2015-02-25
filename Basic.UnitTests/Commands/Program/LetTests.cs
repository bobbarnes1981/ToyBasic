﻿using Basic.Commands.Program;
using Basic.Expressions;
using Moq;
using NUnit.Framework;

namespace Basic.UnitTests.Commands.Program
{
    [TestFixture]
    public class LetTests
    {
        [Test]
        public void Let_ConstructedObject_HasCorrectKeyword()
        {
            string variable = "variable";

            Mock<IExpression> expressionMock = new Mock<IExpression>();

            Let underTest = new Let(variable, expressionMock.Object);

            Assert.That(underTest.Keyword, Is.EqualTo(Keywords.Let));
        }

        [Test]
        public void Let_ConstructedObject_HasCorrectIsSystemValue()
        {
            string variable = "variable";

            Mock<IExpression> expressionMock = new Mock<IExpression>();

            Let underTest = new Let(variable, expressionMock.Object);

            Assert.That(underTest.IsSystem, Is.False);
        }

        [Test]
        public void Let_ConstructedObject_HasCorrectTextRepresentation()
        {
            string variable = "variable";
            string expressionText = "my expresison text";
            int expressionValue = 10;

            Mock<IExpression> expressionMock = new Mock<IExpression>();
            expressionMock.Setup(x => x.Text).Returns(expressionText);

            Let underTest = new Let(variable, expressionMock.Object);

            Assert.That(underTest.Text, Is.EqualTo(string.Format("{0} {1} = {2}", Keywords.Let, variable, expressionText)));
        }

        [Test]
        public void Let_Execute_CallsCorrectInterfaceMethod()
        {
            string variable = "variable";
            string value = "10";

            Mock<IHeap> heapMock = new Mock<IHeap>();

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();
            interpreterMock.Setup(x => x.Heap).Returns(heapMock.Object);

            Mock<IExpression> expressionMock = new Mock<IExpression>();
            expressionMock.Setup(x => x.Result()).Returns(value);

            Let underTest = new Let(variable, expressionMock.Object);

            underTest.Execute(interpreterMock.Object);

            interpreterMock.Verify(x => x.Heap, Times.Once);

            heapMock.Verify(x => x.Set(variable, value), Times.Once);
        }
    }
}