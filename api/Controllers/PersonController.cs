using MediatR;
using Microsoft.AspNetCore.Mvc;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Queries;
using StargateAPI.Models;
using System.Net;

namespace StargateAPI.Controllers
{
   
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PersonController> _logger;
        public PersonController(IMediator mediator, ILogger<PersonController> logger)
        {
            _mediator = mediator;
            _logger = logger;

        }

        [HttpGet]
        public async Task<IActionResult> GetPeople()
        {
            try
            {
                var result = await _mediator.Send(new GetPeople());

                return this.GetResponse(result, _logger);
            }
            catch (Exception ex)
            {
                return this.GetResponse(ex, _logger);
            }
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetPersonByName(string name)
        {
            try
            {
                var result = await _mediator.Send(new GetPersonByName()
                {
                    Name = name
                });

                return this.GetResponse(result, _logger);
            }
            catch (Exception ex)
            {
                return this.GetResponse(ex, _logger);
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> CreatePerson([FromBody] CreatePersonRequest request)
        {
            try
            {
                var result = await _mediator.Send(new CreatePerson()
                {
                    Name = request.Name
                });

                return this.GetResponse(result, _logger);
            }
            catch (Exception ex)
            {
                return this.GetResponse(ex, _logger);
            }

        }
    }
}