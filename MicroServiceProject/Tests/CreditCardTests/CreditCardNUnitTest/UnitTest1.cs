using MP.Infrastructure.Helper;
using NUnit.Framework;
using System.IO;

namespace CreditCardNUnitTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            string[] allfiles = Directory.GetFiles(@"C:\Users\husey\Documents\GitHub\NetCoreMicroServiceProject", "*.json", SearchOption.AllDirectories);
            Assert.True(1 == 1);
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}