using RpgRoguelikeRun.Configuration;

namespace RpgRoguelikeRun.WorldLayer.Locations;

public class VillageFactory : LocationFactory
{
    public override Location Create(string name)
        => new Location(name, LocationType.Village, MarketFactory.CreateVillageMarket());
}