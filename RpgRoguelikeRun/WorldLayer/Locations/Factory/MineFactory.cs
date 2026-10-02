using RpgRoguelikeRun.Configuration;

namespace RpgRoguelikeRun.WorldLayer.Locations.Factory;
public class MineFactory : LocationFactory
{
    public override Location Create(string name)
        => new Location(name, LocationType.Mine, MarketFactory.CreateMineMarket());
}