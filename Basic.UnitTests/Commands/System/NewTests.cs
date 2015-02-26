using Basic.Commands.System;
using Moq;
using NUnit.Framework;

namespace Basic.UnitTests.Commands.System
{
    [TestFixture]
    public class NewTests
    {
        [Test]
        public void New_ConstructedObject_HasCorrectKeyword()
        {
            New underTest = new New();

            Assert.That(underTest.Keyword, Is.EqualTo(Keywords.New));
        }

        [Test]
        public void New_ConstructedObject_HasCorrectIsSystemValue()
        {
            New underTest = new New();

            Assert.That(underTest.IsSystem, Is.True);
        }

        [Test]
        public void New_ConstructedObject_HasCorrectTextRepresentation()
        {
            New underTest = new New();

            Assert.That(underTest.Text, Is.EqualTo(Keywords.New.ToString()));
        }

        [Test]
        public void New_Execute_CallsCorrectInterfaceMethod()
        {
            Mock<IHeap> heapMock = new Mock<IHeap>();

            Mock<ILineBuffer> bufferMock = new Mock<ILineBuffer>();

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();
            interpreterMock.Setup(x => x.Buffer).Returns(bufferMock.Object);
            interpreterMock.Setup(x => x.Heap).Returns(heapMock.Object);

            New underTest = new New();
            underTest.Execute(interpreterMock.Object);

            interpreterMock.Verify(x => x.Heap, Times.Once);

            heapMock.Verify(x => x.Clear(), Times.Once);

            interpreterMock.Verify(x => x.Buffer, Times.Once);

            bufferMock.Verify(x => x.Clear(), Times.Once);
        }
    }
}
