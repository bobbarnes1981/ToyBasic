using Basic.Commands;
using Basic.Commands.Program;
using Basic.Factories;
using Moq;
using NUnit.Framework;
using Basic.Expressions;
using Basic.Types;

namespace Basic.UnitTests.Commands.Program
{
    [TestFixture]
    public class ForTests
    {
        [Test]
        public void For_ConstructedObject_HasCorrectKeyword()
        {
            For underTest = new For(null, null, null, null);

            Assert.That(underTest.Keyword, Is.EqualTo(Keywords.For));
        }

        [Test]
        public void For_ConstructedObject_HasCorrectIsSystemValue()
        {
            For underTest = new For(null, null, null, null);

            Assert.That(underTest.IsSystem, Is.False);
        }

        [Test]
        public void For_ConstructedObject_HasCorrectTextRepresentation()
        {
            Variable variable = new Variable("variable", null);
            
            Value start =new Value(new Number(1));
            Value end = new Value(new Number(10));
            Value step = new Value(new Number(1));

            For underTest = new For(variable, start, end, step);

            Assert.That(underTest.Text, Is.EqualTo("For $variable = 1 To 10 Step 1"));
        }

        [Test]
        public void For_Execute_CallsCorrectInterfaceMethod()
        {
            Variable variable = new Variable("variable", null);

            Value start = new Value(new Number(1));
            Value end = new Value(new Number(10));
            Value step = new Value(new Number(1));

            Mock<ICommand> commandMock = new Mock<ICommand>();

            Mock<IFrame> frameMock = new Mock<IFrame>();

            Mock<IStack> stackMock = new Mock<IStack>();
            stackMock.Setup(x => x.Push()).Returns(frameMock.Object);

            Mock<IHeap> heapMock = new Mock<IHeap>();

            Mock<ILineBuffer> bufferMock = new Mock<ILineBuffer>();
            bufferMock.Setup(x => x.Current).Returns(new Line(10, commandMock.Object));

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();
            interpreterMock.Setup(x => x.Buffer).Returns(bufferMock.Object);
            interpreterMock.Setup(x => x.Stack).Returns(stackMock.Object);
            interpreterMock.Setup(x => x.Heap).Returns(heapMock.Object);

            For underTest = new For(variable, start, end, step);

            underTest.Execute(interpreterMock.Object);

            frameMock.Verify(x => x.Set("for_end", 10), Times.Once);
            frameMock.Verify(x => x.Set("for_step", 1), Times.Once);
            frameMock.Verify(x => x.Set("for_line", 10), Times.Once);
            frameMock.Verify(x => x.Set("for_var", "variable"), Times.Once);

            interpreterMock.Verify(x => x.Buffer, Times.Once);

            bufferMock.Verify(x => x.Current, Times.Once);

            stackMock.Verify(x => x.Push(), Times.Once);
        }
    }
}
