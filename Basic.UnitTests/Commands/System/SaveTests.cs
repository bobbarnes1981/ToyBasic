using System.Collections.Generic;
using Basic.Commands;
using Basic.Commands.System;
using Basic.Types;
using Moq;
using NUnit.Framework;

namespace Basic.UnitTests.Commands.System
{
    [TestFixture]
    public class SaveTests
    {
        [Test]
        public void Save_ConstructedObject_HasCorrectKeyword()
        {
            String filename = new String("filename.ext");

            Save underTest = new Save(filename);

            Assert.That(underTest.Keyword, Is.EqualTo(Keywords.Save));
        }

        [Test]
        public void Save_ConstructedObject_HasCorrectIsSystemValue()
        {
            String filename = new String("filename.ext");

            Save underTest = new Save(filename);

            Assert.That(underTest.IsSystem, Is.True);
        }

        [Test]
        public void Save_ConstructedObject_HasCorrectTextRepresentation()
        {
            String filename = new String("filename.ext");

            Save underTest = new Save(filename);

            Assert.That(underTest.Text, Is.EqualTo(string.Format("{0} {1}", Keywords.Save, filename)));
        }

        [Test]
        public void Save_Execute_CallsCorrectInterfaceMethod()
        {
            String filename = new String("filename.ext");

            string text = "a line of text";

            Mock<ICommand> commandMock = new Mock<ICommand>();
            commandMock.Setup(x => x.Text).Returns(text);

            Mock<ILine> lineMock = new Mock<ILine>();
            lineMock.Setup(x => x.Command).Returns(commandMock.Object);

            Mock<ILineBuffer> bufferMock = new Mock<ILineBuffer>();
            bufferMock.SetupSequence(x => x.End).Returns(false).Returns(true);
            bufferMock.Setup(x => x.Current).Returns(lineMock.Object);

            Mock<IStorage> storageMock = new Mock<IStorage>();

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();
            interpreterMock.Setup(x => x.Buffer).Returns(bufferMock.Object);
            interpreterMock.Setup(x => x.Storage).Returns(storageMock.Object);

            Save underTest = new Save(filename);
            underTest.Execute(interpreterMock.Object);
            
            bufferMock.Verify(x => x.Reset(), Times.Once);

            interpreterMock.Verify(x => x.Buffer, Times.Exactly(5));

            bufferMock.Verify(x => x.End, Times.Exactly(2));

            interpreterMock.Verify(x => x.Storage, Times.Once);

            storageMock.Verify(x => x.Save((string)filename.Value(), It.IsAny<IEnumerable<string>>()), Times.Once);
        }
    }
}
