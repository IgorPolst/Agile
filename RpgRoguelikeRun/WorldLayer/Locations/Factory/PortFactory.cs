using RpgRoguelikeRun.Enums;
using RpgRoguelikeRun.WorldLayer.Regions;

namespace RpgRoguelikeRun.WorldLayer.Locations.Factory;

public class PortFactory : LocationFactory
{
    public override Location Create(string name, IRegionStrategy region)
        => new Location(name, LocationType.Port, region,
                        MarketFactory.CreatePortMarket(region));
}