using System;
using System.Collections.Generic;
using Basic.Commands;
using Moq;
using NUnit.Framework;

namespace Basic.UnitTests
{
    [TestFixture]
    public class BufferTests
    {
        [Test]
        public void Buffer_Add_Null()
        {
            Buffer underTest = new Buffer();

            Assert.Throws<ArgumentNullException>(() => underTest.Add(null));
        }

        [Test]
        public void Buffer_Add_LineInvalidNumber()
        {
            Buffer underTest = new Buffer();

            Mock<ILine> lineMock = new Mock<ILine>();
            lineMock.Setup(x => x.Number).Returns(0);

            Assert.Throws<Errors.Buffer>(() => underTest.Add(lineMock.Object), "Invalid line number '0'");
        }

        [Test]
        public void Buffer_Add_LineNullCommand()
        {
            Buffer underTest = new Buffer();

            Mock<ILine> lineMock = new Mock<ILine>();
            lineMock.Setup(x => x.Number).Returns(10);
            lineMock.Setup(x => x.Command).Returns<ICommand>(null);

            Assert.Throws<Errors.Buffer>(() => underTest.Add(lineMock.Object), "Invalid line command 'null'");
        }

        [Test]
        public void Buffer_Add_LineSystemCommand()
        {
            Buffer underTest = new Buffer();

            Mock<ICommand> commandMock = new Mock<ICommand>();
            commandMock.Setup(x => x.IsSystem).Returns(true);
            commandMock.Setup(x => x.Keyword).Returns(Keywords.Clear);

            Mock<ILine> lineMock = new Mock<ILine>();
            lineMock.Setup(x => x.Number).Returns(10);
            lineMock.Setup(x => x.Command).Returns(commandMock.Object);

            Assert.Throws<Errors.Buffer>(() => underTest.Add(lineMock.Object), string.Format("System command '{0}' cannot be added to buffer", Keywords.Clear));
        }

        [Test]
        public void Buffer_Add_Line()
        {
            Buffer underTest = new Buffer();

            Mock<ICommand> commandMock = new Mock<ICommand>();

            Mock<ILine> lineMock = new Mock<ILine>();
            lineMock.Setup(x => x.Number).Returns(10);
            lineMock.Setup(x => x.Command).Returns(commandMock.Object);

            underTest.Add(lineMock.Object);
        }

        [Test]
        public void Buffer_Renumber_Empty()
        {
            Buffer underTest = new Buffer();

            underTest.Renumber();
        }

        [Test]
        public void Buffer_Renumber_LinesNumbers()
        {
            Buffer underTest = new Buffer();

            for (int i = 1; i < 5; i++)
            {
                Mock<ICommand> commandMock = new Mock<ICommand>();
                commandMock.Setup(x => x.IsSystem).Returns(false);
                underTest.Add(new Line(i, commandMock.Object));
            }

            underTest.Renumber();
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
        public void Buffer_Renumber_LinesData()
        {
            Buffer underTest = new Buffer();

            Mock<Basic.Expressions.IExpression> expressionMock = new Mock<Basic.Expressions.IExpression>();
            expressionMock.Setup(x => x.Result()).Returns(true);

            // goto
            underTest.Add(
                new Line(
                    1,
                    new Basic.Commands.Program.Goto(2)));

            // if
            underTest.Add(
                new Line(
                    2, 
                    new Basic.Commands.Program.If(
                        null,
                        new Basic.Commands.Program.Goto(10))));

            // nested if
            underTest.Add(
                new Line(
                    10,
                    new Basic.Commands.Program.If(
                        null,
                        new Basic.Commands.Program.If(
                            null,
                            new Basic.Commands.Program.Goto(1)))));

            underTest.Renumber();
            underTest.Reset();

            // goto
            underTest.Next();
            Assert.That(underTest.Current.Number, Is.EqualTo(10));
            Assert.That(((Basic.Commands.Program.Goto)underTest.Current.Command).LineNumber, Is.EqualTo(20));

            // if
            underTest.Next();
            Assert.That(underTest.Current.Number, Is.EqualTo(20));
            Assert.That(((Basic.Commands.Program.Goto)((Basic.Commands.Program.If)underTest.Current.Command).Command).LineNumber, Is.EqualTo(30));

            // nested if
            underTest.Next();
            Assert.That(underTest.Current.Number, Is.EqualTo(30));
            Assert.That(((Basic.Commands.Program.Goto)((Basic.Commands.Program.If)((Basic.Commands.Program.If)underTest.Current.Command).Command).Command).LineNumber, Is.EqualTo(30));
        }

        [Test]
        public void Buffer_Jump_ValidLineNumber()
        {
            Buffer underTest = new Buffer();

            for (int i = 10; i < 50; i += 10)
            {
                Mock<ICommand> commandMock = new Mock<ICommand>();
                commandMock.Setup(x => x.IsSystem).Returns(false);
                underTest.Add(new Line(i, commandMock.Object));
            }

            underTest.Renumber();
            underTest.Reset();

            underTest.Next();
            Assert.That(underTest.Current.Number, Is.EqualTo(10));

            underTest.Jump(40);
            underTest.Next();

            Assert.That(underTest.Current.Number, Is.EqualTo(40));
        }

        [Test]
        public void Buffer_Jump_InvalidLineNumber()
        {
            Buffer underTest = new Buffer();

            for (int i = 10; i < 50; i += 10)
            {
                Mock<ICommand> commandMock = new Mock<ICommand>();
                commandMock.Setup(x => x.IsSystem).Returns(false);
                underTest.Add(new Line(i, commandMock.Object));
            }

            underTest.Renumber();
            underTest.Reset();

            underTest.Next();
            Assert.That(underTest.Current.Number, Is.EqualTo(10));

            Assert.Throws<Errors.Buffer>(() => underTest.Jump(80));
        }
    }
}
