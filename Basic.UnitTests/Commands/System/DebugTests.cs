using Basic.Commands.System;
using Moq;
using NUnit.Framework;

namespace Basic.UnitTests.Commands.System
{
    [TestFixture]
    public class DebugTests
    {
        [Test]
        public void Debug_ConstructedObject_HasCorrectKeyword()
        {
            Debug underTest = new Debug();

            Assert.That(underTest.Keyword, Is.EqualTo(Keywords.Debug));
        }

        [Test]
        public void Debug_ConstructedObject_HasCorrectIsSystemValue()
        {
            Debug underTest = new Debug();

            Assert.That(underTest.IsSystem, Is.True);
        }

        [Test]
        public void Debug_ConstructedObject_HasCorrectTextRepresentation()
        {
            Debug underTest = new Debug();

            Assert.That(underTest.Text, Is.EqualTo(Keywords.Debug.ToString()));
        }

        [Test]
        public void Debug_Execute_CallsCorrectInterfaceMethod()
        {
            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            Debug underTest = new Debug();
            underTest.Execute(interpreterMock.Object);

            interpreterMock.Verify(x => x.Execute(true), Times.Once);
        }
    }
}
