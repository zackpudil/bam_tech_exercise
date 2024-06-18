using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
using StargateAPI.Business.Dtos;
using StargateAPI.Controllers;

namespace StargateAPI.Business.Queries
{
    public class GetAstronautDutiesByName : IRequest<GetAstronautDutiesByNameResult>
    {
        public string Name { get; set; } = string.Empty;
    }

    public class GetAstronautDutiesByNameHandler : IRequestHandler<GetAstronautDutiesByName, GetAstronautDutiesByNameResult>
    {
        private readonly StargateContext _context;
        private readonly IMediator _mediator;

        public GetAstronautDutiesByNameHandler(StargateContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<GetAstronautDutiesByNameResult> Handle(GetAstronautDutiesByName request, CancellationToken cancellationToken)
        {

            var result = new GetAstronautDutiesByNameResult();
            var person = (await _mediator.Send(new GetPersonByName() { Name = request.Name }))?.Person;

            if (person is null)
            {
                throw new BadHttpRequestException($"Person with name {request.Name} not found", 404);
            }

            result.Person = person;

            var duties = _context.AstronautDuties.AsNoTracking().Where(ad => ad.PersonId == person.PersonId);

            result.AstronautDuties = duties.ToList();

            return result;

        }
    }

    public class GetAstronautDutiesByNameResult : BaseResponse
    {
        public PersonAstronaut? Person { get; set; }
        public List<AstronautDuty> AstronautDuties { get; set; } = new List<AstronautDuty>();
    }
}
