using System.IO;
using FluentAssertions;
using Google.Protobuf;
using Google.Protobuf.Examples.AddressBook;
using NUnit.Framework;

namespace Protobuf.CodeGen.BuildTask.Tests
{
    [TestFixture]
    public class TestProtos
    {
        [Test]
        public void TestProtoGen()
        {
            AddressBook ab = new AddressBook();
            ab.People.Add(new Person
            {
                Id = 0,
                Name = "Jhon",
            });

            using (var ms = new MemoryStream())
            {
                ab.WriteTo(ms);
                ms.Position = 0;
                var bytes = ms.ToArray();
                var ab2 = AddressBook.Parser.ParseFrom(bytes);
                ab2.People.Count.Should().Be(1);
                ab2.People[0].Id.Should().Be(ab.People[0].Id);
                ab2.People[0].Name.Should().Be(ab.People[0].Name);
            }
        }
    }
}
