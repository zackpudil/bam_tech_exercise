using Dapper;
using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
using StargateAPI.Business.Queries;
using StargateAPI.Models;
using System.Net;

namespace StargateAPI.Business.Commands
{
    public class CreateAstronautDuty : IRequest<CreateAstronautDutyResult>
    {
        public required string Name { get; set; }

        public required string Rank { get; set; }

        public required string DutyTitle { get; set; }

        public DateTime DutyStartDate { get; set; }
    }

    public class CreateAstronautDutyPreProcessor : IRequestPreProcessor<CreateAstronautDuty>
    {
        private readonly StargateContext _context;

        public CreateAstronautDutyPreProcessor(StargateContext context)
        {
            _context = context;
        }

        public Task Process(CreateAstronautDuty request, CancellationToken cancellationToken)
        {
            var person = _context.People
                .AsNoTracking()
                .FirstOrDefault(z => z.Name == request.Name);

            if (person is null)
                throw new BadHttpRequestException($"Person with name {request.Name} not found", 404);

            var verifyNoPreviousDuty = _context.AstronautDuties
                .AsNoTracking()
                .FirstOrDefault(z => z.DutyTitle == request.DutyTitle && z.DutyStartDate == request.DutyStartDate);

            if (verifyNoPreviousDuty is not null) 
                throw new BadHttpRequestException($"Duty already exists {request.DutyTitle}({request.DutyStartDate})", 400);

            return Task.CompletedTask;
        }
    }

    public class CreateAstronautDutyHandler : IRequestHandler<CreateAstronautDuty, CreateAstronautDutyResult>
    {
        private readonly StargateContext _context;
        private readonly ILogger<CreateAstronautDutyHandler> _logger;

        public CreateAstronautDutyHandler(StargateContext context, ILogger<CreateAstronautDutyHandler> logger)
        {
            _context = context;
            _logger = logger;

        }
        public async Task<CreateAstronautDutyResult> Handle(CreateAstronautDuty request, CancellationToken cancellationToken)
        {

            var person = _context.People
                .Include(x => x.AstronautDetail)
                .Include(x => x.AstronautDuties)
                .First(p => p.Name == request.Name);

            

            if (person.AstronautDetail == null)
            {
                person.AstronautDetail = new AstronautDetail()
                {
                    PersonId = person.Id,
                    CurrentDutyTitle = request.DutyTitle,
                    CurrentRank = request.Rank,
                    CareerStartDate = request.DutyStartDate.Date,
                    CareerEndDate = request.DutyTitle == "RETIRED" ? request.DutyStartDate.Date.AddDays(-1) : null
                };

            }
            else
            {
                person.AstronautDetail.CurrentDutyTitle = request.DutyTitle;
                person.AstronautDetail.CurrentRank = request.Rank;
                if (request.DutyTitle == "RETIRED")
                {
                    person.AstronautDetail.CareerEndDate = request.DutyStartDate.AddDays(-1).Date;
                }
            }

            var astronautDuty = person.AstronautDuties.OrderByDescending(ad => ad.DutyStartDate).FirstOrDefault();

            if (astronautDuty != null)
            {
                _logger.LogInformation($"Ending Astronaut duty {astronautDuty.DutyTitle} for {person.Name} ({person.Id})");
                astronautDuty.DutyEndDate = request.DutyStartDate.AddDays(-1).Date;
            }

            var newAstronautDuty = new AstronautDuty()
            {
                PersonId = person.Id,
                Rank = request.Rank,
                DutyTitle = request.DutyTitle,
                DutyStartDate = request.DutyStartDate.Date,
                DutyEndDate = null
            };

            person.AstronautDuties.Add(newAstronautDuty);

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Creating Astronaut duty {person.AstronautDetail.CurrentDutyTitle} for {person.Name} ({person.Id})");

            return new CreateAstronautDutyResult()
            {
                Id = newAstronautDuty.Id
            };
        }
    }

    public class CreateAstronautDutyResult : BaseResponse
    {
        public int? Id { get; set; }
    }
}
