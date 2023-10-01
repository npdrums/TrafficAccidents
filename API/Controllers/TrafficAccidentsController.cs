using API.Contracts;

using AutoMapper;

using Domain.Interfaces;
using Domain.Models;

using Microsoft.AspNetCore.Mvc;

using AccidentType = API.Contracts.Enums.AccidentType;
using ParticipantsNominalCount = API.Contracts.Enums.ParticipantsNominalCount;
using ParticipantsStatus = API.Contracts.Enums.ParticipantsStatus;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
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
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<TrafficAccidentResponse>> CreateTrafficAccident([FromBody] TrafficAccidentRequest trafficAccidentRequest)
    {
        var trafficAccident = _mapper.Map<TrafficAccident>(trafficAccidentRequest);

        var savedTrafficAccident = await _trafficAccidentsService.CreateTrafficAccident(trafficAccident);

        if (savedTrafficAccident is null) return BadRequest("Oops! Something went wrong!");

        var trafficAccidentResponse = _mapper.Map<TrafficAccidentResponse>(savedTrafficAccident);

        return Created($"{trafficAccidentResponse.TrafficAccidentId}",trafficAccidentResponse);
    }

    [HttpPut]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<TrafficAccidentResponse>> UpdateTrafficAccident(
        [FromBody] TrafficAccidentUpdateRequest trafficAccidentUpdateRequest)
    {
        var trafficAccident = _mapper.Map<TrafficAccident>(trafficAccidentUpdateRequest);

        var savedTrafficAccident = await _trafficAccidentsService.UpdateTrafficAccident(trafficAccident);

        if (savedTrafficAccident is null) return BadRequest("Oops! Something went wrong!");

        var trafficAccidentResponse = _mapper.Map<TrafficAccidentResponse>(savedTrafficAccident);

        return Ok(trafficAccidentResponse);
    }

    [HttpPatch("{trafficAccidentId:guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<TrafficAccidentResponse>> UpdateTrafficAccidentDescription(
        Guid trafficAccidentId, [FromBody] string description)
    {
        var trafficAccident = await _trafficAccidentsService.UpdateTrafficAccidentDescription(trafficAccidentId, description);

        if (trafficAccident is null) return BadRequest("Oops! Something went wrong!");

        var trafficAccidentResponse = _mapper.Map<TrafficAccidentResponse>(trafficAccident);

        return Ok(trafficAccidentResponse);
    }

    [HttpGet("{trafficAccidentId:guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<IReadOnlyList<TrafficAccidentResponse>>> GetTrafficAccidentById(Guid trafficAccidentId)
    {
        var trafficAccident = await _trafficAccidentsService.GetTrafficAccidentsById(trafficAccidentId);

        if (trafficAccident is null) return NotFound("Sorry! Not found!");

        var trafficAccidentResponse = _mapper.Map<TrafficAccidentResponse>(trafficAccident);
        
        return Ok(trafficAccidentResponse);
    }

    [HttpGet("{caseNumber}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<IReadOnlyList<TrafficAccidentResponse>>> GetTrafficAccidentsByCaseNumber(string caseNumber)
    {
        var trafficAccident = await _trafficAccidentsService.GetTrafficAccidentsByCaseNumber(caseNumber);

        if (trafficAccident.Count == 0) return NotFound("Sorry! Not found!");

        var trafficAccidentsResponse = _mapper.Map<IReadOnlyList<TrafficAccidentResponse>>(trafficAccident);

        return Ok(trafficAccidentsResponse);
    }

    [HttpGet("participants-counts")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<IReadOnlyList<ParticipantsNominalCount>>> GetParticipantsNominalCountList()
    {
        var participantsNominalCountsResponse = await _trafficAccidentsService.GetParticipantsNominalCounts();

        return Ok(participantsNominalCountsResponse);
    }

    [HttpGet("participant-statuses")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<IReadOnlyList<ParticipantsStatus>>> GetParticipantsStatusList()
    {
        var participantsStatusesResponse = await _trafficAccidentsService.GetParticipantsStatuses();

        return Ok(participantsStatusesResponse);
    }

    [HttpGet("accident-types")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<IReadOnlyList<AccidentType>>> GetAccidentTypeList()
    {
        var accidentTypes = await _trafficAccidentsService.GetAccidentTypes();

        return Ok(accidentTypes);
    }

    [HttpDelete("{trafficAccidentId:guid}")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> DeleteTrafficAccident(Guid trafficAccidentId)
    {
        await _trafficAccidentsService.DeleteTrafficAccident(trafficAccidentId);

        return NoContent();
    }
}