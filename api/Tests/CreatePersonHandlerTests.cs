using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.EntityFrameworkCore;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Data;

namespace StargateAPI.Tests
{
    [TestClass]
    public class CreatePersonHandlerTests
    {
        [TestMethod]
        public async Task Handle_ShouldAddPerson()
        {
            var people = new Mock<DbSet<Person>>();
            var context = new Mock<StargateContext>();
            context.Setup(x => x.People).Returns(people.Object);

            var sut = new CreatePersonHandler(context.Object);

            await sut.Handle(new CreatePerson
            {
                Name = "test"
            }, new CancellationToken());

            people.Verify(x => x.AddAsync(It.Is<Person>(p => p.Name == "test"), It.IsAny<CancellationToken>()));
        }
    }
}
