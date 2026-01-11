using API.Models;
using Application.Handlers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Operations.Query.Showcase;

[ApiController]
[Route("[controller]")]
public class ShowcaseController(ILogLinesHandler logLinesHandler, IMapper mapper) : ControllerBase
{
    private readonly ILogLinesHandler _logLinesHandler = logLinesHandler;
    private readonly IMapper _mapper = mapper;

    [HttpGet("get")]
    public async Task<IActionResult> Get()
    {
        var lines = await System.IO.File.ReadAllLinesAsync("Operations/Query/Showcase/match_log.txt");

        var match = await _logLinesHandler.HandleLogsLines(lines);

        return Ok(_mapper.Map<Match>(match));
    }
}