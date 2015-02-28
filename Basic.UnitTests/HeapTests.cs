using Basic.Errors;
using NUnit.Framework;

namespace Basic.UnitTests
{
    [TestFixture]
    public class HeapTests
    {
        [Test]
        public void Heap_Get_ValidName()
        {
            string variableName = "variable";
            int variableValue = 10;

            Heap underTest = new Heap(null);

            underTest.Set(variableName, variableValue);

            Assert.That(underTest.Get(variableName), Is.EqualTo(variableValue));
        }

        [Test]
        public void Heap_Get_InvalidName()
        {
            Heap underTest = new Heap(null);

            Assert.Throws<HeapError>(() => underTest.Get("variable"));
        }
    }
}
