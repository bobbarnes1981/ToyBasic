using Basic.Commands.Program;
using Basic.Expressions;
using Moq;
using NUnit.Framework;
using Basic.Types;

namespace Basic.UnitTests.Commands.Program
{
    [TestFixture]
    public class GotoTests
    {
        [Test]
        public void Goto_ConstructedObject_HasCorrectKeyword()
        {
            Goto underTest = new Goto(new Number(0));

            Assert.That(underTest.Keyword, Is.EqualTo(Keywords.Goto));
        }

        [Test]
        public void Goto_ConstructedObject_HasCorrectIsSystemValue()
        {
            Goto underTest = new Goto(new Number(0));

            Assert.That(underTest.IsSystem, Is.False);
        }

        [Test]
        public void Goto_ConstructedObject_HasCorrectTextRepresentation()
        {
            Number line = new Number(0);

            Goto underTest = new Goto(line);

            Assert.That(underTest.Text, Is.EqualTo(string.Format("{0} {1}", Keywords.Goto, line.Value())));
        }

        [Test]
        public void Goto_Execute_CallsCorrectInterfaceMethod()
        {
            Number line = new Number(0);

            Mock<ILineBuffer> bufferMock = new Mock<ILineBuffer>();

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();
            interpreterMock.Setup(x => x.Buffer).Returns(bufferMock.Object);

            Goto underTest = new Goto(line);

            underTest.Execute(interpreterMock.Object);

            interpreterMock.Verify(x => x.Buffer, Times.Once);

            bufferMock.Verify(x => x.Jump((int)line.Value()), Times.Once);
        }
    }
}
