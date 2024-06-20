using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
using StargateAPI.Models;

namespace StargateAPI.Business.Commands
{
    public class CreatePerson : IRequest<CreatePersonResult>
    {
        public required string Name { get; set; } = string.Empty;
    }

    public class CreatePersonPreProcessor : IRequestPreProcessor<CreatePerson>
    {
        private readonly StargateContext _context;
        public CreatePersonPreProcessor(StargateContext context)
        {
            _context = context;
        }
        public Task Process(CreatePerson request, CancellationToken cancellationToken)
        {
            var person = _context.People.AsNoTracking().FirstOrDefault(z => z.Name == request.Name);

            if (person is not null) throw new BadHttpRequestException($"Person with name {request.Name} already exists", 409);

            return Task.CompletedTask;
        }
    }

    public class CreatePersonHandler : IRequestHandler<CreatePerson, CreatePersonResult>
    {
        private readonly StargateContext _context;
        private readonly ILogger<CreatePersonHandler> _logger;

        public CreatePersonHandler(StargateContext context, ILogger<CreatePersonHandler> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<CreatePersonResult> Handle(CreatePerson request, CancellationToken cancellationToken)
        {

            var newPerson = new Person()
            {
                Name = request.Name
            };

            await _context.People.AddAsync(newPerson);

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Created person {newPerson.Name}({newPerson.Id})");

            return new CreatePersonResult()
            {
                Id = newPerson.Id
            };
          
        }
    }

    public class CreatePersonResult : BaseResponse
    {
        public int Id { get; set; }
    }
}
