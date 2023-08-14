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
    public async Task<ActionResult<TrafficAccident>> CreateTrafficAccident([FromBody] TrafficAccidentRequest trafficAccidentRequest)
    {
        var watch = new System.Diagnostics.Stopwatch();
        watch.Start();

        var trafficAccident = _mapper.Map<TrafficAccident>(trafficAccidentRequest);

        var savedTrafficAccident= await _trafficAccidentsService.CreateTrafficAccident(trafficAccident);

        if (savedTrafficAccident is null) return BadRequest("Oops! Something went wrong!");

        var trafficAccidentResponse = _mapper.Map<TrafficAccidentResponse>(savedTrafficAccident);

        Console.WriteLine($"Execution time: {watch.ElapsedMilliseconds}");

        return Ok(trafficAccidentResponse);
    }

    [HttpGet("{externalId}")]
    public async Task<ActionResult<TrafficAccidentResponse>> GetTrafficAccident(string externalId)
    {
        var trafficAccident = await _trafficAccidentsService.GetTrafficAccidentByExternalId(externalId);

        if (trafficAccident is null) return NotFound("Sorry! Not found!");

        var trafficAccidentResponse = _mapper.Map<TrafficAccidentResponse>(trafficAccident);
        
        return Ok(trafficAccidentResponse);
    }
}