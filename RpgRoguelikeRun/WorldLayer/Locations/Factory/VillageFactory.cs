using RpgRoguelikeRun.WorldLayer.Regions;
using RpgRoguelikeRun.Enums;

namespace RpgRoguelikeRun.WorldLayer.Locations.Factory;

public class VillageFactory : LocationFactory
{
    public override Location Create(string name, IRegionStrategy region)
        => new Location(name, LocationType.Village, region,
                        MarketFactory.CreateVillageMarket(region));
}