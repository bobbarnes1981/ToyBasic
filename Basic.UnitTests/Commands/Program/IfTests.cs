﻿using Basic.Commands;
using Basic.Commands.Program;
using Basic.Expressions;
using Moq;
using NUnit.Framework;

namespace Basic.UnitTests.Commands.Program
{
    [TestFixture]
    public class IfTests
    {
        [Test]
        public void If_ConstructedObject_HasCorrectKeyword()
        {
            Mock<IExpression> expressionMock = new Mock<IExpression>();

            Mock<ICommand> commandMock = new Mock<ICommand>();

            If underTest = new If(expressionMock.Object, commandMock.Object);

            Assert.That(underTest.Keyword, Is.EqualTo(Keywords.If));
        }

        [Test]
        public void If_ConstructedObject_HasCorrectIsSystemValue()
        {
            Mock<IExpression> expressionMock = new Mock<IExpression>();

            Mock<ICommand> commandMock = new Mock<ICommand>();

            If underTest = new If(expressionMock.Object, commandMock.Object);

            Assert.That(underTest.IsSystem, Is.False);
        }

        [Test]
        public void If_ConstructedObject_HasCorrectTextRepresentation()
        {
            string expressionText = "my expression text";
            Mock<IExpression> expressionMock = new Mock<IExpression>();
            expressionMock.Setup(x => x.Text).Returns(expressionText);

            string commandText = "my command text";
            Mock<ICommand> commandMock = new Mock<ICommand>();
            commandMock.Setup(x => x.Text).Returns(commandText);

            If underTest = new If(expressionMock.Object, commandMock.Object);

            Assert.That(underTest.Text, Is.EqualTo(string.Format("{0} {1} {2} {3}", Keywords.If, expressionText, Keywords.Then, commandText)));
        }

        [Test]
        public void If_Execute_CallsCorrectInterfaceMethodWhenTrue()
        {
            Mock<IExpression> expressionMock = new Mock<IExpression>();
            expressionMock.Setup(x => x.Result()).Returns(true);

            Mock<ICommand> commandMock = new Mock<ICommand>();

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            If underTest = new If(expressionMock.Object, commandMock.Object);
            underTest.Execute(interpreterMock.Object);

            expressionMock.Verify(x => x.Result(), Times.Once);

            commandMock.Verify(x => x.Execute(It.IsAny<IInterpreter>()), Times.Once);
        }

        [Test]
        public void If_Execute_CallsCorrectInterfaceMethodWhenFalse()
        {
            Mock<IExpression> expressionMock = new Mock<IExpression>();
            expressionMock.Setup(x => x.Result()).Returns(false);

            Mock<ICommand> commandMock = new Mock<ICommand>();

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            If underTest = new If(expressionMock.Object, commandMock.Object);
            underTest.Execute(interpreterMock.Object);

            expressionMock.Verify(x => x.Result(), Times.Once);

            commandMock.Verify(x => x.Execute(It.IsAny<IInterpreter>()), Times.Never);
        }
    }
}
