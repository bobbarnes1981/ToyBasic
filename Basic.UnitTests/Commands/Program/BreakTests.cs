using Basic.Commands.Program;
using Moq;
using NUnit.Framework;

namespace Basic.UnitTests.Commands.Program
{
    [TestFixture]
    public class BreakTests
    {
        [Test]
        public void Break_ConstructedObject_HasCorrectKeyword()
        {
            Break underTest = new Break();

            Assert.That(underTest.Keyword, Is.EqualTo(Keywords.Break));
        }

        [Test]
        public void Break_ConstructedObject_HasCorrectIsSystemValue()
        {
            Break underTest = new Break();

            Assert.That(underTest.IsSystem, Is.False);
        }

        [Test]
        public void Break_ConstructedObject_HasCorrectTextRepresentation()
        {
            Break underTest = new Break();

            Assert.That(underTest.Text, Is.EqualTo(Keywords.Break.ToString()));
        }

        [Test]
        public void Break_Execute_CallsCorrectInterfaceMethod()
        {
            Mock<IConsole> consoleMock = new Mock<IConsole>();

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();
            interpreterMock.Setup(x => x.Console).Returns(consoleMock.Object);

            Break underTest = new Break();
            underTest.Execute(interpreterMock.Object);
        }
    }
}
