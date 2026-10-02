using RpgRoguelikeRun.Configuration;

namespace RpgRoguelikeRun.WorldLayer.Locations;

public class PortFactory : LocationFactory
{
    public override Location Create(string name)
        => new Location(name, LocationType.Port, MarketFactory.CreatePortMarket());
}