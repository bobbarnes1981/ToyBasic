using Basic.Commands.Program;
using Basic.Expressions;
using Basic.Types;
using Moq;
using NUnit.Framework;

namespace Basic.UnitTests.Commands.Program
{
    [TestFixture]
    public class LetTests
    {
        [Test]
        public void Let_ConstructedObject_HasCorrectKeyword()
        {
            Variable variable = new Variable(null, "variable", new Value(new Number(-1)));

            Mock<INode> expressionMock = new Mock<INode>();

            Let underTest = new Let(variable, expressionMock.Object);

            Assert.That(underTest.Keyword, Is.EqualTo(Keywords.Let));
        }

        [Test]
        public void Let_ConstructedObject_HasCorrectIsSystemValue()
        {
            Variable variable = new Variable(null, "variable", new Value(new Number(-1)));

            Mock<INode> expressionMock = new Mock<INode>();

            Let underTest = new Let(variable, expressionMock.Object);

            Assert.That(underTest.IsSystem, Is.False);
        }

        [Test]
        public void Let_ConstructedObject_HasCorrectTextRepresentation()
        {
            Variable variable = new Variable(null, "variable", new Value(new Number(-1)));
            string expressionText = "my expresison text";

            Mock<INode> expressionMock = new Mock<INode>();
            expressionMock.Setup(x => x.Text).Returns(expressionText);

            Let underTest = new Let(variable, expressionMock.Object);

            Assert.That(underTest.Text, Is.EqualTo(string.Format("{0} {1} = {2}", Keywords.Let, variable.Text, expressionText)));
        }

        [Test]
        public void Let_Execute_CallsCorrectInterfaceMethod()
        {
            string value = "10";

            Mock<IHeap> heapMock = new Mock<IHeap>();

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();
            interpreterMock.Setup(x => x.Heap).Returns(heapMock.Object);

            Mock<INode> expressionMock = new Mock<INode>();
            expressionMock.Setup(x => x.Result()).Returns(value);

            Variable variable = new Variable(interpreterMock.Object, "variable", new Value(new Number(-1)));

            Let underTest = new Let(variable, expressionMock.Object);

            underTest.Execute(interpreterMock.Object);

            interpreterMock.Verify(x => x.Heap, Times.Once);

            heapMock.Verify(x => x.Set(variable.Name, value), Times.Once);
        }
    }
}
