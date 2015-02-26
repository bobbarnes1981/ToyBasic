using Basic.Commands.Program;
using Basic.Types;
using Moq;
using NUnit.Framework;

namespace Basic.UnitTests.Commands.Program
{
    [TestFixture]
    public class InputTests
    {
        [Test]
        public void Input_ConstructedObject_HasCorrectKeyword()
        {
            Variable variable = new Variable(null, "variable");

            Input underTest = new Input(variable);

            Assert.That(underTest.Keyword, Is.EqualTo(Keywords.Input));
        }

        [Test]
        public void Input_ConstructedObject_HasCorrectIsSystemValue()
        {
            Variable variable = new Variable(null, "variable");

            Input underTest = new Input(variable);

            Assert.That(underTest.IsSystem, Is.False);
        }

        [Test]
        public void Input_ConstructedObject_HasCorrectTextRepresentation()
        {
            Variable variable = new Variable(null, "variable");

            Input underTest = new Input(variable);

            Assert.That(underTest.Text, Is.EqualTo(string.Format("{0} {1}", Keywords.Input, variable.Text)));
        }

        [Test]
        public void Input_Execute_CallsCorrectInterfaceMethod()
        {
            string value = "10";

            Mock<IHeap> heapMock = new Mock<IHeap>();

            Mock<IConsole> consoleMock = new Mock<IConsole>();
            consoleMock.Setup(x => x.Input()).Returns(value);

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();
            interpreterMock.Setup(x => x.Heap).Returns(heapMock.Object);
            interpreterMock.Setup(x => x.Console).Returns(consoleMock.Object);

            Variable variable = new Variable(interpreterMock.Object, "variable");

            Input underTest = new Input(variable);

            underTest.Execute(interpreterMock.Object);

            consoleMock.Verify(x => x.Output("? "), Times.Once);

            interpreterMock.Verify(x => x.Heap, Times.Once);

            interpreterMock.Verify(x => x.Console, Times.Exactly(2));

            heapMock.Verify(x => x.Set(variable.Text, value), Times.Once);

            consoleMock.Verify(x => x.Input(), Times.Once);
        }
    }
}
