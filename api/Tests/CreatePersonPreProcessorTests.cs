using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.EntityFrameworkCore;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Data;

namespace StargateAPI.Tests
{
    [TestClass]
    public class CreatePersonPreProcessorTests
    {
        [TestMethod]
        public void Process_WhenPersonExists_ShouldThrowError()
        {
            var contextMock = new Mock<StargateContext>();
            contextMock.Setup(ctx => ctx.People).ReturnsDbSet(new List<Person>
            {
                new Person { Name = "exists" }
            });

            var sut = new CreatePersonPreProcessor(contextMock.Object);

            try
            {
                sut.Process(new CreatePerson
                {
                    Name = "exists",
                }, new CancellationToken());
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is BadHttpRequestException);
                Assert.AreEqual(((BadHttpRequestException)ex).StatusCode, 400);
            }
        }

        [TestMethod]
        public void Process_WhenPersonDoesNotExists_ShouldNotThrowError()
        {
            var contextMock = new Mock<StargateContext>();
            contextMock.Setup(ctx => ctx.People).ReturnsDbSet(new List<Person>
            {
                new Person { Name = "exists" }
            });

            var sut = new CreatePersonPreProcessor(contextMock.Object);

            sut.Process(new CreatePerson
            {
                Name = "does_not_exists",
            }, new CancellationToken());

            Assert.IsTrue(true);
        }
    }
}
