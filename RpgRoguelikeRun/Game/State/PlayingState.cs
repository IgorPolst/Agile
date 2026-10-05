using RpgRoguelikeRun.Entities.Events;
using RpgRoguelikeRun.Enums;
using RpgRoguelikeRun.Services;
using RpgRoguelikeRun.UI;

namespace RpgRoguelikeRun.Game.States;

public class PlayingState : IGameState
{
    public string Name => "Playing";

    public void Enter(GameContext context)
    {
    }

    public bool HandleInput(GameContext context)
    {
        var command = InputHandler.ReadCommand();

        switch (command)
        {
            case GameCommand.Exit:
                context.Finish();
                return true;

            case GameCommand.Step:
                DoTurn(context);
                break;

            case GameCommand.OpenMarket:
                MarketMenu.Open(context.Trader);
                break;

            case GameCommand.Travel:
                TravelFromLocation(context);
                break;
            
            case GameCommand.Pause:
                context.ChangeState(new PauseState());
                break;
        }

        if (IsBankrupt(context))
        {
            context.ChangeState(new GameOverState());
            return false;
        }

        return false;
    }

    public void Render(GameContext context)
    {
        GameRenderer.Render(
            context.Trader,
            context.World,
            context.Difficulty,
            80, 25);
    }

    // ---------- Внутренняя логика ----------

    private static void DoTurn(GameContext context)
    {
        double chance = context.Difficulty switch
        {
            Difficulty.Easy   => 0.20,
            Difficulty.Normal => 0.30,
            Difficulty.Hard   => 0.45,
            _                 => 0.30
        };

        TurnFacade.DoTurn(context.World, context.Trader, chance);
    }

    private static void TravelFromLocation(GameContext context)
    {
        var trader = context.Trader;

        if (trader.CurrentLocation == null)
        {
            Console.WriteLine("Вы уже в пути.");
            Console.ReadKey(true);
            return;
        }

        var road = TravelMenu.ChooseRoad(trader);
        if (road == null) return;

        if (!context.World.TryPayToll(road, trader)) return;

        trader.EnterRoad(road);
        Console.WriteLine($"Вы отправились по {road.Name} → {road.Destination?.Name ?? "?"}.");
        Console.WriteLine($"Длина: {road.TravelTime} ходов.");
        Console.ReadKey(true);
    }

    private static bool IsBankrupt(GameContext context)
    {
        return context.Trader.Gold <= 0
            && context.Trader.Inventory.Stacks.Count == 0;
    }
}