using RpgRoguelikeRun.Enums;
using RpgRoguelikeRun.WorldLayer.Regions;

namespace RpgRoguelikeRun.WorldLayer.Locations.Factory;
public class MineFactory : LocationFactory
{
    public override Location Create(string name, IRegionStrategy region)
        => new Location(name, LocationType.Mine, region,
                        MarketFactory.CreateMineMarket(region));
}