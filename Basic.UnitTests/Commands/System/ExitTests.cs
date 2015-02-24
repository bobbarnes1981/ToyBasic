using Basic.Commands.System;
using Moq;
using NUnit.Framework;

namespace Basic.UnitTests.Commands.System
{
    [TestFixture]
    public class ExitTests
    {
        [Test]
        public void Exit_ConstructedObject_HasCorrectKeyword()
        {
            Exit underTest = new Exit();

            Assert.That(underTest.Keyword, Is.EqualTo(Keywords.Exit));
        }

        [Test]
        public void Exit_ConstructedObject_HasCorrectIsSystemValue()
        {
            Exit underTest = new Exit();

            Assert.That(underTest.IsSystem, Is.True);
        }

        [Test]
        public void Exit_ConstructedObject_HasCorrectTextRepresentation()
        {
            Exit underTest = new Exit();

            Assert.That(underTest.Text, Is.EqualTo(Keywords.Exit.ToString()));
        }

        [Test]
        public void Exit_Execute_CallsCorrectInterfaceMethod()
        {
            Mock<IConsole> consoleMock = new Mock<IConsole>();

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();
            interpreterMock.Setup(x => x.Console).Returns(consoleMock.Object);

            Exit underTest = new Exit();
            underTest.Execute(interpreterMock.Object);

            interpreterMock.Verify(x => x.Exit(), Times.Once);
        }
    }
}
