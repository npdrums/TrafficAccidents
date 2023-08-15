using API.Contracts;

using AutoMapper;

using Domain.Interfaces;
using Domain.Models;

using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrafficAccidentsController : ControllerBase
{
    private readonly ITrafficAccidentsService _trafficAccidentsService;
    private readonly IMapper _mapper;

    public TrafficAccidentsController(ITrafficAccidentsService trafficAccidentsService, IMapper mapper)
    {
        _trafficAccidentsService = trafficAccidentsService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<TrafficAccidentResponse>> CreateTrafficAccident([FromBody] TrafficAccidentRequest trafficAccidentRequest)
    {
        var trafficAccident = _mapper.Map<TrafficAccident>(trafficAccidentRequest);

        var savedTrafficAccident= await _trafficAccidentsService.CreateTrafficAccident(trafficAccident);

        if (savedTrafficAccident is null) return BadRequest("Oops! Something went wrong!");

        var trafficAccidentResponse = _mapper.Map<TrafficAccidentResponse>(savedTrafficAccident);

        return Ok(trafficAccidentResponse);
    }

    [HttpGet("{caseNumber}")]
    public async Task<ActionResult<IReadOnlyList<TrafficAccidentResponse>>> GetTrafficAccidentsByCaseNumber(string caseNumber)
    {
        var trafficAccident = await _trafficAccidentsService.GetTrafficAccidentsByCaseNumber(caseNumber);

        if (trafficAccident.Count == 0) return NotFound("Sorry! Not found!");

        var trafficAccidentResponse = _mapper.Map<IReadOnlyList<TrafficAccidentResponse>>(trafficAccident);
        
        return Ok(trafficAccidentResponse);
    }
    
    [HttpDelete("{externalId}")]
    public async Task<IActionResult> DeleteTrafficAccident(Guid externalId)
    {
        await _trafficAccidentsService.DeleteTrafficAccident(externalId);

        return NoContent();
    }
}