using Basic.Commands.System;
using Moq;
using NUnit.Framework;

namespace Basic.UnitTests.Commands.System
{
    [TestFixture]
    public class RenumberTests
    {
        [Test]
        public void Renumber_ConstructedObject_HasCorrectKeyword()
        {
            Renumber underTest = new Renumber();

            Assert.That(underTest.Keyword, Is.EqualTo(Keywords.Renumber));
        }

        [Test]
        public void Renumber_ConstructedObject_HasCorrectIsSystemValue()
        {
            Renumber underTest = new Renumber();

            Assert.That(underTest.IsSystem, Is.True);
        }

        [Test]
        public void Renumber_ConstructedObject_HasCorrectTextRepresentation()
        {
            Renumber underTest = new Renumber();

            Assert.That(underTest.Text, Is.EqualTo(Keywords.Renumber.ToString()));
        }

        [Test]
        public void Renumber_Execute_CallsCorrectInterfaceMethod()
        {
            Mock<ILineBuffer> bufferMock = new Mock<ILineBuffer>();

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();
            interpreterMock.Setup(x => x.Buffer).Returns(bufferMock.Object);

            Renumber underTest = new Renumber();
            underTest.Execute(interpreterMock.Object);

            interpreterMock.Verify(x => x.Buffer, Times.Once);

            bufferMock.Verify(x => x.Renumber(It.IsAny<IInterpreter>()), Times.Once);
        }
    }
}
