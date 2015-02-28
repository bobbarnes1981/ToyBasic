using System;
using Basic.Commands;
using Basic.Errors;
using Basic.Types;
using Moq;
using NUnit.Framework;

namespace Basic.UnitTests
{
    [TestFixture]
    public class LineBufferTests
    {
        [Test]
        public void LineBuffer_Add_Null()
        {
            LineBuffer underTest = new LineBuffer();

            Assert.Throws<ArgumentNullException>(() => underTest.Add(null));
        }

        [Test]
        public void LineBuffer_Add_LineInvalidNumber()
        {
            LineBuffer underTest = new LineBuffer();

            Mock<ILine> lineMock = new Mock<ILine>();
            lineMock.Setup(x => x.Number).Returns(0);

            Assert.Throws<LineBufferError>(() => underTest.Add(lineMock.Object), "Invalid line number '0'");
        }

        [Test]
        public void LineBuffer_Add_LineNullCommand()
        {
            LineBuffer underTest = new LineBuffer();

            Mock<ILine> lineMock = new Mock<ILine>();
            lineMock.Setup(x => x.Number).Returns(10);
            lineMock.Setup(x => x.Command).Returns<ICommand>(null);

            Assert.Throws<LineBufferError>(() => underTest.Add(lineMock.Object), "Invalid line command 'null'");
        }

        [Test]
        public void LineBuffer_Add_LineSystemCommand()
        {
            LineBuffer underTest = new LineBuffer();

            Mock<ICommand> commandMock = new Mock<ICommand>();
            commandMock.Setup(x => x.IsSystem).Returns(true);
            commandMock.Setup(x => x.Keyword).Returns(Keywords.Clear);

            Mock<ILine> lineMock = new Mock<ILine>();
            lineMock.Setup(x => x.Number).Returns(10);
            lineMock.Setup(x => x.Command).Returns(commandMock.Object);

            Assert.Throws<LineBufferError>(() => underTest.Add(lineMock.Object), string.Format("System command '{0}' cannot be added to buffer", Keywords.Clear));
        }

        [Test]
        public void LineBuffer_Add_Line()
        {
            LineBuffer underTest = new LineBuffer();

            Mock<ICommand> commandMock = new Mock<ICommand>();

            Mock<ILine> lineMock = new Mock<ILine>();
            lineMock.Setup(x => x.Number).Returns(10);
            lineMock.Setup(x => x.Command).Returns(commandMock.Object);

            underTest.Add(lineMock.Object);
        }

        [Test]
        public void LineBuffer_Renumber_Empty()
        {
            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            LineBuffer underTest = new LineBuffer();

            underTest.Renumber(interpreterMock.Object);
        }

        [Test]
        public void LineBuffer_Renumber_LinesNumbers()
        {
            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            LineBuffer underTest = new LineBuffer();

            for (int i = 1; i < 5; i++)
            {
                Mock<ICommand> commandMock = new Mock<ICommand>();
                commandMock.Setup(x => x.IsSystem).Returns(false);
                underTest.Add(new Line(i, commandMock.Object));
            }

            underTest.Renumber(interpreterMock.Object);
            underTest.Reset();

            int counter = 10;
            while (!underTest.End)
            {
                underTest.Next();
                Assert.That(underTest.Current.Number, Is.EqualTo(counter));
                counter += 10;
            }
        }

        [Test]
        public void LineBuffer_Renumber_LinesData()
        {
            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            LineBuffer underTest = new LineBuffer();

            Mock<Basic.Expressions.INode> expressionMock = new Mock<Basic.Expressions.INode>();
            expressionMock.Setup(x => x.Result(It.IsAny<IInterpreter>())).Returns(true);

            // goto
            underTest.Add(
                new Line(
                    1,
                    new Basic.Commands.Program.Goto(new Number(2))));

            // if
            underTest.Add(
                new Line(
                    2, 
                    new Basic.Commands.Program.If(
                        null,
                        new Basic.Commands.Program.Goto(new Number(10)))));

            // nested if
            underTest.Add(
                new Line(
                    10,
                    new Basic.Commands.Program.If(
                        null,
                        new Basic.Commands.Program.If(
                            null,
                            new Basic.Commands.Program.Goto(new Number(1))))));

            underTest.Renumber(interpreterMock.Object);
            underTest.Reset();

            // goto
            underTest.Next();
            Assert.That(underTest.Current.Number, Is.EqualTo(10));
            Assert.That(((Basic.Commands.Program.Goto)underTest.Current.Command).LineNumber.Value(It.IsAny<IInterpreter>()), Is.EqualTo(20));

            // if
            underTest.Next();
            Assert.That(underTest.Current.Number, Is.EqualTo(20));
            Assert.That(((Basic.Commands.Program.Goto)((Basic.Commands.Program.If)underTest.Current.Command).Command).LineNumber.Value(It.IsAny<IInterpreter>()), Is.EqualTo(30));

            // nested if
            underTest.Next();
            Assert.That(underTest.Current.Number, Is.EqualTo(30));
            Assert.That(((Basic.Commands.Program.Goto)((Basic.Commands.Program.If)((Basic.Commands.Program.If)underTest.Current.Command).Command).Command).LineNumber.Value(It.IsAny<IInterpreter>()), Is.EqualTo(30));
        }

        [Test]
        public void LineBuffer_Jump_ValidLineNumber()
        {
            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            LineBuffer underTest = new LineBuffer();

            for (int i = 10; i < 50; i += 10)
            {
                Mock<ICommand> commandMock = new Mock<ICommand>();
                commandMock.Setup(x => x.IsSystem).Returns(false);
                underTest.Add(new Line(i, commandMock.Object));
            }

            underTest.Renumber(interpreterMock.Object);
            underTest.Reset();

            underTest.Next();
            Assert.That(underTest.Current.Number, Is.EqualTo(10));

            underTest.Jump(40);
            underTest.Next();

            Assert.That(underTest.Current.Number, Is.EqualTo(40));
        }

        [Test]
        public void LineBuffer_Jump_InvalidLineNumber()
        {
            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();

            LineBuffer underTest = new LineBuffer();

            for (int i = 10; i < 50; i += 10)
            {
                Mock<ICommand> commandMock = new Mock<ICommand>();
                commandMock.Setup(x => x.IsSystem).Returns(false);
                underTest.Add(new Line(i, commandMock.Object));
            }

            underTest.Renumber(interpreterMock.Object);
            underTest.Reset();

            underTest.Next();
            Assert.That(underTest.Current.Number, Is.EqualTo(10));

            Assert.Throws<LineBufferError>(() => underTest.Jump(80));
        }
    }
}
