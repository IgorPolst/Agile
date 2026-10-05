using RpgRoguelikeRun.Configuration;
using RpgRoguelikeRun.Entities;
using RpgRoguelikeRun.Enums;
using RpgRoguelikeRun.WorldLayer;

namespace RpgRoguelikeRun.Services;

public static class GameSetupFacade
{
    public static (World world, Trader trader) StartNewGame(
        int mapWidth,
        int mapHeight,
        Difficulty difficulty,
        string traderName = "Ганс",
        int startingGold = 100)
    {
        var world = new World(mapWidth, mapHeight);
        WorldConfigurator.Configure(world, difficulty);

        var trader = TraderFactory.CreateStartingTrader(traderName, startingGold);
        world.AddTrader(trader);

        if (world.Locations.Count == 0)
            throw new InvalidOperationException("WorldConfigurator не создал ни одной локации.");

        trader.EnterLocation(world.Locations.First());

        return (world, trader);
    }
}