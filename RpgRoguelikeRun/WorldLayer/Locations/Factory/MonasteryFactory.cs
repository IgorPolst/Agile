using RpgRoguelikeRun.Enums;
using RpgRoguelikeRun.WorldLayer.Regions;

namespace RpgRoguelikeRun.WorldLayer.Locations.Factory;
public class MonasteryFactory : LocationFactory
{
    public override Location Create(string name, IRegionStrategy region)
        => new Location(name, LocationType.Monastery, region,
                        MarketFactory.CreatePortMarket(region));
}