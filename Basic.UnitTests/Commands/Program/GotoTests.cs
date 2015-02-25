using Basic.Commands.Program;
using Basic.Expressions;
using Moq;
using NUnit.Framework;

namespace Basic.UnitTests.Commands.Program
{
    [TestFixture]
    public class GotoTests
    {
        [Test]
        public void Goto_ConstructedObject_HasCorrectKeyword()
        {
            Goto underTest = new Goto(0);

            Assert.That(underTest.Keyword, Is.EqualTo(Keywords.Goto));
        }

        [Test]
        public void Goto_ConstructedObject_HasCorrectIsSystemValue()
        {
            Goto underTest = new Goto(0);

            Assert.That(underTest.IsSystem, Is.False);
        }

        [Test]
        public void Goto_ConstructedObject_HasCorrectTextRepresentation()
        {
            int line = 0;

            Goto underTest = new Goto(line);

            Assert.That(underTest.Text, Is.EqualTo(string.Format("{0} {1}", Keywords.Goto, line)));
        }

        [Test]
        public void Goto_Execute_CallsCorrectInterfaceMethod()
        {
            int line = 0;

            Mock<IBuffer> bufferMock = new Mock<IBuffer>();

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();
            interpreterMock.Setup(x => x.Buffer).Returns(bufferMock.Object);

            Goto underTest = new Goto(line);

            underTest.Execute(interpreterMock.Object);

            interpreterMock.Verify(x => x.Buffer, Times.Once);

            bufferMock.Verify(x => x.Jump(line), Times.Once);
        }
    }
}
