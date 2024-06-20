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
    public class AstronautDutyController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AstronautDutyController> _logger;
        public AstronautDutyController(IMediator mediator, ILogger<AstronautDutyController> logger)
        {
            _mediator = mediator;
            _logger = logger;

        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetAstronautDutiesByName([FromRoute] string name)
        {
            try
            {
                var result = await _mediator.Send(new GetAstronautDutiesByName()
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
        public async Task<IActionResult> CreateAstronautDuty([FromBody] CreateAstronautDutyRequest request)
        {
            try
            {
                var result = await _mediator.Send(new CreateAstronautDuty
                {
                    Name = request.Name,
                    Rank = request.Rank,
                    DutyTitle = request.DutyTitle,
                    DutyStartDate = request.DutyStartDate
                });

                return this.GetResponse(result, _logger);
            }
            catch(Exception ex)
            {
                return this.GetResponse(ex, _logger);
            }
        }
    }
}