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

        // add null command

        // add system command

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
        public void Buffer_Renumber_Lines()
        {
            Buffer underTest = new Buffer();

            List<Mock<ILine>> lineMocks = new List<Mock<ILine>>();
            for (int i = 1; i < 5; i++)
            {
                Mock<ILine> lineMock = new Mock<ILine>();
                lineMock.Setup(x => x.Number).Returns(i);
                lineMocks.Add(lineMock);
            }

            underTest.Renumber();

            int counter = 10;
            while (!underTest.End)
            {
                Assert.That(underTest.Current, Is.EqualTo(counter));
                Assert.That(underTest.Fetch.Number, Is.EqualTo(counter));
                counter += 10;
            }
        }

        // check that goto, if, nested if statements are renumbered correctly
    }
}
