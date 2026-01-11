using Domain.Match;

namespace Application.Handlers;

public interface ILogLinesHandler
{
    Task<MatchAggregate> HandleLogsLines(IEnumerable<string> logLines);
}