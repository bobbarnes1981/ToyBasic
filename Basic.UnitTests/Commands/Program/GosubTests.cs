using Basic.Commands;
using Basic.Commands.Program;
using Moq;
using NUnit.Framework;
using Basic.Types;

namespace Basic.UnitTests.Commands.Program
{
    [TestFixture]
    public class GosubTests
    {
        [Test]
        public void Gosub_ConstructedObject_HasCorrectKeyword()
        {
            Gosub underTest = new Gosub(new Number(0));

            Assert.That(underTest.Keyword, Is.EqualTo(Keywords.Gosub));
        }

        [Test]
        public void Gosub_ConstructedObject_HasCorrectIsSystemValue()
        {
            Gosub underTest = new Gosub(new Number(0));

            Assert.That(underTest.IsSystem, Is.False);
        }

        [Test]
        public void Gosub_ConstructedObject_HasCorrectTextRepresentation()
        {
            Number line = new Number(0);

            Gosub underTest = new Gosub(line);

            Assert.That(underTest.Text, Is.EqualTo(string.Format("{0} {1}", Keywords.Gosub, 0)));
        }

        [Test]
        public void Gosub_Execute_CallsCorrectInterfaceMethod()
        {
            Number line = new Number(20);

            Mock<ICommand> commandMock = new Mock<ICommand>();

            Mock<IStack> stackMock = new Mock<IStack>();

            Mock<ILineBuffer> bufferMock = new Mock<ILineBuffer>();
            bufferMock.Setup(x => x.Current).Returns(new Line(10, commandMock.Object));

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();
            interpreterMock.Setup(x => x.Buffer).Returns(bufferMock.Object);
            interpreterMock.Setup(x => x.Stack).Returns(stackMock.Object);

            Gosub underTest = new Gosub(line);

            underTest.Execute(interpreterMock.Object);

            interpreterMock.Verify(x => x.Buffer, Times.Exactly(2));

            bufferMock.Verify(x => x.Current, Times.Once);

            stackMock.Verify(x => x.Push(It.IsAny<IFrame>()), Times.Once);

            bufferMock.Verify(x => x.Jump(20), Times.Once);
        }
    }
}
