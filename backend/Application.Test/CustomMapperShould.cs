using Application.Mapping;
using Domain.Round;
using Xunit;

namespace Application.Test;

public class CustomMapperShould
{
    [Fact]
    public void MapTerrorist()
    {
        var result = CustomMapper.GameLogSideToDomainSide("TERRORIST");

        Assert.Equal(Side.Terrorist, result);
    }

    [Fact]
    public void MapCounterTerrorist()
    {
        var result = CustomMapper.GameLogSideToDomainSide("CT");

        Assert.Equal(Side.CounterTerrorist, result);
    }
}