using RpgRoguelikeRun.Configuration;

namespace RpgRoguelikeRun.WorldLayer.Locations.Factory;

public class TownFactory : LocationFactory
{
    public override Location Create(string name)
        => new Location(name, LocationType.Town, MarketFactory.CreateTownMarket());
}