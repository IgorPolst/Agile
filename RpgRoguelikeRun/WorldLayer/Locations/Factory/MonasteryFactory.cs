using RpgRoguelikeRun.Configuration;

namespace RpgRoguelikeRun.WorldLayer.Locations;
public class MonasteryFactory : LocationFactory
{
    public override Location Create(string name)
        => new Location(name, LocationType.Monastery, MarketFactory.CreateMonasteryMarket());
}