using RpgRoguelikeRun.Entities;
using RpgRoguelikeRun.WorldLayer;
using RpgRoguelikeRun.Services.Random;

namespace RpgRoguelikeRun.Services;

public static class TurnFacade
{

    public static bool DoTurn(World world, Trader trader, double eventChance)
    {
        if (trader.CurrentRoad != null)
        {
            bool arrived = trader.StepOnRoad();

            if (GameRandom.NextDouble() < eventChance)
                world.TriggerRandomRoadEvent(trader, trader.CurrentRoad);

            if (arrived)
                world.ArriveAt(trader);
        }
        else
        {
            trader.TryMove();
        }

        bool bankrupt = trader.Gold <= 0 && trader.Inventory.Stacks.Count == 0;
        return !bankrupt;
    }
}