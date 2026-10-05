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
            int steps = 1 + trader.BonusSteps;

            bool arrived = false;
            for (int i = 0; i < steps; i++)
            {
                arrived = trader.StepOnRoad();
                if (arrived) break;
            }

            if (GameRandom.NextDouble() < eventChance)
                world.TriggerRandomRoadEvent(trader, trader.CurrentRoad);

            if (arrived)
                world.ArriveAt(trader);
        }
        else
        {
            trader.TryMove();
        }

        trader.ResetTurnBonuses();

        bool bankrupt = trader.Gold <= 0 && trader.Inventory.Stacks.Count == 0;
        return !bankrupt;
    }
}