using Basic.Commands.System;
using Moq;
using NUnit.Framework;

namespace Basic.UnitTests.Commands.System
{
    [TestFixture]
    public class ClearTests
    {
        [Test]
        public void Clear_ConstructedObject_HasCorrectKeyword()
        {
            Clear underTest = new Clear();

            Assert.That(underTest.Keyword, Is.EqualTo(Keywords.Clear));
        }

        [Test]
        public void Clear_ConstructedObject_HasCorrectIsSystemValue()
        {
            Clear underTest = new Clear();

            Assert.That(underTest.IsSystem, Is.True);
        }

        [Test]
        public void Clear_ConstructedObject_HasCorrectTextRepresentation()
        {
            Clear underTest = new Clear();

            Assert.That(underTest.Text, Is.EqualTo(Keywords.Clear.ToString()));
        }

        [Test]
        public void Clear_Execute_CallsCorrectInterfaceMethod()
        {
            Mock<IConsole> consoleMock = new Mock<IConsole>();

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();
            interpreterMock.Setup(x => x.Console).Returns(consoleMock.Object);

            Clear underTest = new Clear();
            underTest.Execute(interpreterMock.Object);

            interpreterMock.Verify(x => x.Console, Times.Once);

            consoleMock.Verify(x => x.Clear(), Times.Once);
        }
    }
}
