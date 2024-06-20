using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.EntityFrameworkCore;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Data;

namespace StargateAPI.Tests
{
    [TestClass]
    public class CreateAstronautDutyHandlerTests
    {
        [TestMethod]
        public async Task Handle_WhenAstronautDetailsDoNotExist_WillCreateOneAndCreateDuty()
        {
            var data = new List<Person>
            {
                new Person
                {
                    Id = 1,
                    Name = "one"
                }
            };

            var context = new Mock<StargateContext>();
            context.Setup(x => x.People).ReturnsDbSet(data);

            var logger = new Mock<ILogger<CreateAstronautDutyHandler>>();

            var sut = new CreateAstronautDutyHandler(context.Object, logger.Object);


            await sut.Handle(new CreateAstronautDuty
            {
                Name = "one",
                DutyTitle = "title",
                Rank = "E1",
                DutyStartDate = DateTime.Today
            }, new CancellationToken());

            Assert.IsNotNull(data.First().AstronautDetail);
            Assert.AreEqual(data.First().AstronautDuties.Count(), 1);

            Assert.AreEqual(expected: data.First().AstronautDetail.CurrentDutyTitle, "title");
            Assert.AreEqual(expected: data.First().AstronautDetail.CurrentRank, "E1");
            Assert.AreEqual(expected: data.First().AstronautDetail.CareerStartDate, DateTime.Today);
        }

        [TestMethod]
        public async Task Handle_WhenAstronautDutyExists_WillUpdateExistingEndDate()
        {
            var data = new List<Person>
            {
                new Person
                {
                    Id = 1,
                    Name = "one",
                    AstronautDetail = new AstronautDetail
                    {
                        CareerStartDate = DateTime.Today.AddDays(-2),
                        CurrentDutyTitle = "existing_title",
                        CurrentRank = "current_rank",
                    },
                    AstronautDuties = new List<AstronautDuty>
                    {
                        new AstronautDuty
                        {
                            DutyStartDate = DateTime.Today.AddDays(-2),
                            DutyTitle = "existing_title",
                            Rank = "current_rank"
                        }
                    }
                }
            };

            var context = new Mock<StargateContext>();
            context.Setup(x => x.People).ReturnsDbSet(data);

            var logger = new Mock<ILogger<CreateAstronautDutyHandler>>();
            var sut = new CreateAstronautDutyHandler(context.Object, logger.Object);


            await sut.Handle(new CreateAstronautDuty
            {
                Name = "one",
                DutyTitle = "title",
                Rank = "E1",
                DutyStartDate = DateTime.Today
            }, new CancellationToken());

            Assert.IsNotNull(data.First().AstronautDetail);
            Assert.AreEqual(data.First().AstronautDuties.Count(), 2);

            Assert.AreEqual(expected: data.First().AstronautDetail.CurrentDutyTitle, "title");
            Assert.AreEqual(expected: data.First().AstronautDetail.CurrentRank, "E1");
            Assert.AreEqual(expected: data.First().AstronautDetail.CareerStartDate, DateTime.Today.AddDays(-2));

            Assert.IsTrue(data.First().AstronautDuties.Any(x => x.DutyEndDate == DateTime.Today.AddDays(-1)));
        }

        [TestMethod]
        public async Task Handle_WhenAstronautDutyDoesNotExistsAndRetired_ShouldUpdateCarrierEndDate()
        {
            var data = new List<Person>
            {
                new Person
                {
                    Id = 1,
                    Name = "one"
                }
            };

            var context = new Mock<StargateContext>();
            context.Setup(x => x.People).ReturnsDbSet(data);

            var logger = new Mock<ILogger<CreateAstronautDutyHandler>>();

            var sut = new CreateAstronautDutyHandler(context.Object, logger.Object);


            await sut.Handle(new CreateAstronautDuty
            {
                Name = "one",
                DutyTitle = "RETIRED",
                Rank = "E1",
                DutyStartDate = DateTime.Today
            }, new CancellationToken());

            Assert.AreEqual(expected: data.First().AstronautDetail.CareerEndDate, DateTime.Today.AddDays(-1));
        }

        [TestMethod]
        public async Task Handle_WhenAstronautDutyExistsAndRetired_ShouldUpdateCarrierEndDate()
        {
            var data = new List<Person>
            {
                new Person
                {
                    Id = 1,
                    Name = "one",
                    AstronautDetail = new AstronautDetail
                    {
                        CareerStartDate = DateTime.Today.AddDays(-2),
                        CurrentDutyTitle = "existing_title",
                        CurrentRank = "current_rank",
                    },
                    AstronautDuties = new List<AstronautDuty>
                    {
                        new AstronautDuty
                        {
                            DutyStartDate = DateTime.Today.AddDays(-2),
                            DutyTitle = "existing_title",
                            Rank = "current_rank"
                        }
                    }
                }
            };

            var context = new Mock<StargateContext>();
            context.Setup(x => x.People).ReturnsDbSet(data);

            var logger = new Mock<ILogger<CreateAstronautDutyHandler>>();

            var sut = new CreateAstronautDutyHandler(context.Object, logger.Object);


            await sut.Handle(new CreateAstronautDuty
            {
                Name = "one",
                DutyTitle = "RETIRED",
                Rank = "E1",
                DutyStartDate = DateTime.Today
            }, new CancellationToken());

            Assert.AreEqual(expected: data.First().AstronautDetail.CareerEndDate, DateTime.Today.AddDays(-1));
        }
    }
}
