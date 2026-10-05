using RpgRoguelikeRun.UI;

namespace RpgRoguelikeRun.Game.States;

public class MenuState : IGameState
{
    public string Name => "Menu";

    public void Enter(GameContext context)
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════╗");
        Console.WriteLine("║    MEDIEVAL TRADER           ║");
        Console.WriteLine("║    Средневековый торговец    ║");
        Console.WriteLine("╚══════════════════════════════╝");
        Console.WriteLine();
        Console.WriteLine("  [Enter] — начать игру");
        Console.WriteLine("  [Escape] — выйти");
    }

    public bool HandleInput(GameContext context)
    {
        var key = Console.ReadKey(true).Key;

        if (key == ConsoleKey.Enter)
        {
            context.InitializeWorld(80, 25);
            context.ChangeState(new PlayingState());
        }
        else if (key == ConsoleKey.Escape)
        {
            context.Finish();
            return true;
        }

        return false;
    }

    public void Render(GameContext context)
    {
        // Меню рисуется один раз в Enter — Render не нужен
    }
}