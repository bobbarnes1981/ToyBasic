using Basic.Commands;
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
            Mock<INode> expressionMock = new Mock<INode>();

            Mock<ICommand> commandMock = new Mock<ICommand>();

            If underTest = new If(expressionMock.Object, commandMock.Object);

            Assert.That(underTest.Keyword, Is.EqualTo(Keywords.If));
        }

        [Test]
        public void If_ConstructedObject_HasCorrectIsSystemValue()
        {
            Mock<INode> expressionMock = new Mock<INode>();

            Mock<ICommand> commandMock = new Mock<ICommand>();

            If underTest = new If(expressionMock.Object, commandMock.Object);

            Assert.That(underTest.IsSystem, Is.False);
        }

        [Test]
        public void If_ConstructedObject_HasCorrectTextRepresentation()
        {
            string expressionText = "my expression text";
            Mock<INode> expressionMock = new Mock<INode>();
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
            Mock<INode> expressionMock = new Mock<INode>();
            expressionMock.Setup(x => x.Result(It.IsAny<IInterpreter>())).Returns(true);

            Mock<ICommand> commandMock = new Mock<ICommand>();

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            If underTest = new If(expressionMock.Object, commandMock.Object);
            underTest.Execute(interpreterMock.Object);

            expressionMock.Verify(x => x.Result(interpreterMock.Object), Times.Once);

            commandMock.Verify(x => x.Execute(It.IsAny<IInterpreter>()), Times.Once);
        }

        [Test]
        public void If_Execute_CallsCorrectInterfaceMethodWhenFalse()
        {
            Mock<INode> expressionMock = new Mock<INode>();
            expressionMock.Setup(x => x.Result(It.IsAny<IInterpreter>())).Returns(false);

            Mock<ICommand> commandMock = new Mock<ICommand>();

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            If underTest = new If(expressionMock.Object, commandMock.Object);
            underTest.Execute(interpreterMock.Object);

            expressionMock.Verify(x => x.Result(interpreterMock.Object), Times.Once);

            commandMock.Verify(x => x.Execute(It.IsAny<IInterpreter>()), Times.Never);
        }
    }
}
