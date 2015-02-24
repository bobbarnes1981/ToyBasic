using Basic.Commands;
using Basic.Commands.System;
using Moq;
using NUnit.Framework;

namespace Basic.UnitTests.Commands.System
{
    [TestFixture]
    public class ListTests
    {
        [Test]
        public void List_ConstructedObject_HasCorrectKeyword()
        {
            Basic.Commands.System.List underTest = new Basic.Commands.System.List();

            Assert.That(underTest.Keyword, Is.EqualTo(Keywords.List));
        }

        [Test]
        public void List_ConstructedObject_HasCorrectIsSystemValue()
        {
            Basic.Commands.System.List underTest = new Basic.Commands.System.List();

            Assert.That(underTest.IsSystem, Is.True);
        }

        [Test]
        public void List_ConstructedObject_HasCorrectTextRepresentation()
        {
            Basic.Commands.System.List underTest = new Basic.Commands.System.List();

            Assert.That(underTest.Text, Is.EqualTo(Keywords.List.ToString()));
        }

        [Test]
        public void List_Execute_CallsCorrectInterfaceMethod()
        {
            int expectedLineNumber = 10;
            string expectedCommandText = "command text";

            Mock<ICommand> command = new Mock<ICommand>();
            command.Setup(x => x.Text).Returns(expectedCommandText);

            Line expectedLine = new Line(expectedLineNumber, command.Object);

            Mock<IConsole> consoleMock = new Mock<IConsole>();

            Mock<IBuffer> bufferMock = new Mock<IBuffer>();
            bufferMock.SetupSequence(x => x.End).Returns(false).Returns(true);
            bufferMock.Setup(x => x.Fetch).Returns(expectedLine);

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();
            interpreterMock.Setup(x => x.Console).Returns(consoleMock.Object);
            interpreterMock.Setup(x => x.Buffer).Returns(bufferMock.Object);

            Basic.Commands.System.List underTest = new Basic.Commands.System.List();
            underTest.Execute(interpreterMock.Object);

            interpreterMock.Verify(x => x.Console, Times.Once);

            interpreterMock.Verify(x => x.Buffer, Times.Exactly(4));

            bufferMock.Verify(x => x.Reset(), Times.Once);

            bufferMock.Verify(x => x.End, Times.Exactly(2));

            consoleMock.Verify(x => x.Output(string.Format("{0} {1}\r\n", expectedLineNumber, expectedCommandText)), Times.Once);
        }
    }
}
