namespace RpgRoguelikeRun.Game.States;

public class GameOverState : IGameState
{
    public string Name => "GameOver";

    public void Enter(GameContext context)
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════╗");
        Console.WriteLine("║      💀 ИГРА ОКОНЧЕНА         ║");
        Console.WriteLine("╚══════════════════════════════╝");
        Console.WriteLine();
        Console.WriteLine($"Вы обанкротились!");
        Console.WriteLine($"Событий пережито: {context.Trader.EventLog.Count}");
        Console.WriteLine($"Сделок совершено: {context.Trader.TradeHistory.Count}");
        Console.WriteLine();
        Console.WriteLine("  [Enter] — выйти");
    }

    public bool HandleInput(GameContext context)
    {
        var key = Console.ReadKey(true).Key;

        if (key == ConsoleKey.Enter)
        {
            context.Finish();
            return true;
        }

        return false;
    }

    public void Render(GameContext context)
    {
    }
}