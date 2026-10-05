namespace RpgRoguelikeRun.Game.States;

public class PauseState : IGameState
{
    public string Name => "Pause";

    public void Enter(GameContext context)
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════╗");
        Console.WriteLine("║           ПАУЗА              ║");
        Console.WriteLine("╚══════════════════════════════╝");
        Console.WriteLine();
        Console.WriteLine("  [Esc] — продолжить");
        Console.WriteLine("  [Q]   — выйти из игры");
    }

    public bool HandleInput(GameContext context)
    {
        var key = Console.ReadKey(true).Key;

        if (key == ConsoleKey.Escape)
        {
            context.ChangeState(new PlayingState());
        }
        else if (key == ConsoleKey.Q)
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