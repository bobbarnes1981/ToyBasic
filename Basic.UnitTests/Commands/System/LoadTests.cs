﻿using Basic.Commands.System;
using Basic.Types;
using Moq;
using NUnit.Framework;

namespace Basic.UnitTests.Commands.System
{
    [TestFixture]
    public class LoadTests
    {
        [Test]
        public void Load_ConstructedObject_HasCorrectKeyword()
        {
            String filename = new String("filename.ext");

            Load underTest = new Load(filename);

            Assert.That(underTest.Keyword, Is.EqualTo(Keywords.Load));
        }

        [Test]
        public void Load_ConstructedObject_HasCorrectIsSystemValue()
        {
            String filename = new String("filename.ext");

            Load underTest = new Load(filename);

            Assert.That(underTest.IsSystem, Is.True);
        }

        [Test]
        public void Load_ConstructedObject_HasCorrectTextRepresentation()
        {
            String filename = new String("filename.ext");

            Load underTest = new Load(filename);

            Assert.That(underTest.Text, Is.EqualTo(string.Format("{0} {1}", Keywords.Load, filename.Text)));
        }

        [Test]
        public void Load_Execute_CallsCorrectInterfaceMethod()
        {
            string filenameString = "filename";
            String filename = new String(filenameString);

            string text = "a line of text";

            Mock<ILineBuffer> bufferMock = new Mock<ILineBuffer>();

            Mock<IStorage> storageMock = new Mock<IStorage>();
            storageMock.Setup(x => x.Load(filenameString)).Returns(new string[] { text });

            Mock<IInterpreter> interpreterMock = new Mock<IInterpreter>();
            interpreterMock.Setup(x => x.Buffer).Returns(bufferMock.Object);
            interpreterMock.Setup(x => x.Storage).Returns(storageMock.Object);

            Load underTest = new Load(filename);
            underTest.Execute(interpreterMock.Object);

            interpreterMock.Verify(x => x.Buffer, Times.Once);

            bufferMock.Verify(x => x.Clear(), Times.Once);

            interpreterMock.Verify(x => x.Storage, Times.Once);

            storageMock.Verify(x => x.Load((string)filename.Value(interpreterMock.Object)), Times.Once);

            interpreterMock.Verify(x => x.ProcessInput(text), Times.Once);
        }
    }
}
