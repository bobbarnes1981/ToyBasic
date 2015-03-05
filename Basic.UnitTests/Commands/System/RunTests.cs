using Basic.Commands.System;
using Moq;
using NUnit.Framework;

namespace Basic.UnitTests.Commands.System
{
    [TestFixture]
    public class RunTests
    {
        [Test]
        public void Run_ConstructedObject_HasCorrectKeyword()
        {
            Run underTest = new Run();

            Assert.That(underTest.Keyword, Is.EqualTo(Keywords.Run));
        }

        [Test]
        public void Run_ConstructedObject_HasCorrectIsSystemValue()
        {
            Run underTest = new Run();

            Assert.That(underTest.IsSystem, Is.True);
        }

        [Test]
        public void Run_ConstructedObject_HasCorrectTextRepresentation()
        {
            Run underTest = new Run();

            Assert.That(underTest.Text, Is.EqualTo(Keywords.Run.ToString()));
        }

        [Test]
        public void Run_Execute_CallsCorrectInterfaceMethod()
        {
            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            Run underTest = new Run();
            underTest.Execute(interpreterMock.Object);

            interpreterMock.Verify(x => x.Execute(false), Times.Once);
        }
    }
}
