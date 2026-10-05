using RpgRoguelikeRun.Enums;
using RpgRoguelikeRun.WorldLayer.Regions;

namespace RpgRoguelikeRun.WorldLayer.Locations.Factory;

public class TownFactory : LocationFactory
{
    public override Location Create(string name, IRegionStrategy region)
        => new Location(name, LocationType.Town, region,
                        MarketFactory.CreateTownMarket(region));
}