using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.EntityFrameworkCore;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Data;

namespace StargateAPI.Tests
{
    [TestClass]
    public class CreateAstronautDutyPreProcessorTests
    {
        [TestMethod]
        public void Process_PersonDoesNotExist_ShouldTHrowError()
        {
            var contextMock = new Mock<StargateContext>();
            contextMock.Setup(ctx => ctx.People).ReturnsDbSet(new List<Person>());

            var sut = new CreateAstronautDutyPreProcessor(contextMock.Object);

            try
            {
                sut.Process(new CreateAstronautDuty
                {
                    Name = "does_not_exist",
                    Rank = "does_not_matter",
                    DutyTitle = "does_not_matter"
                }, new CancellationToken());
            } 
            catch (Exception ex)
            {
                Assert.IsTrue(ex is BadHttpRequestException);
                Assert.AreEqual(((BadHttpRequestException)ex).StatusCode, 404);
            }
        }

        [TestMethod]
        public void Process_ExistingDutyIsFound_ShouldThrowError()
        {
            var contextMock = new Mock<StargateContext>();
            contextMock.Setup(ctx => ctx.People).ReturnsDbSet(new List<Person>
            {
                new Person { Name = "exists" }
            });
            contextMock.Setup(ctx => ctx.AstronautDuties).ReturnsDbSet(new List<AstronautDuty>
            {
                new AstronautDuty
                {
                    DutyTitle = "exists",
                    DutyStartDate = DateTime.Today
                }
            });

            var sut = new CreateAstronautDutyPreProcessor(contextMock.Object);

            try
            {
                sut.Process(new CreateAstronautDuty
                {
                    Name = "exists",
                    Rank = "does_not_matter",
                    DutyTitle = "exists",
                    DutyStartDate = DateTime.Today
                }, new CancellationToken());
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is BadHttpRequestException);
                Assert.AreEqual(((BadHttpRequestException)ex).StatusCode, 400);
            }
        }

        [TestMethod]
        public void Process_ExistingDutyNotIsFound_ShouldThrowError()
        {
            var contextMock = new Mock<StargateContext>();
            contextMock.Setup(ctx => ctx.People).ReturnsDbSet(new List<Person>
            {
                new Person { Name = "exists" }
            });
            contextMock.Setup(ctx => ctx.AstronautDuties).ReturnsDbSet(new List<AstronautDuty>
            {
                new AstronautDuty
                {
                    DutyTitle = "exists",
                    DutyStartDate = DateTime.Today
                }
            });

            var sut = new CreateAstronautDutyPreProcessor(contextMock.Object);
            sut.Process(new CreateAstronautDuty
            {
                Name = "exists",
                Rank = "does_not_matter",
                DutyTitle = "does_not_exists",
                DutyStartDate = DateTime.Today
            }, new CancellationToken());

            Assert.IsTrue(true);
        }
    }
}
