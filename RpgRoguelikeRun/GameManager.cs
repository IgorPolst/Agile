using RpgRoguelikeRun.Enums;
using RpgRoguelikeRun.Game;
using RpgRoguelikeRun.Game.States;

namespace RpgRoguelikeRun;

public sealed class GameManager
{
    private static GameManager? _instance;
    public static GameManager Instance => _instance ??= new GameManager();

    private GameManager()
    {
        Difficulty = Difficulty.Normal;
    }

    public int MapWidth { get; set; } = 80;
    public int MapHeight { get; set; } = 25;
    public Difficulty Difficulty { get; set; }
    public int? Seed { get; set; }

    public void Run()
    {
        Console.CursorVisible = false;

        var context = new GameContext(Difficulty);
        context.ChangeState(new MenuState());

        while (!context.IsFinished)
        {
            context.CurrentState.Render(context);
            context.CurrentState.HandleInput(context);
        }

        Console.CursorVisible = true;
        Console.WriteLine("Спасибо за игру!");
        Console.ReadKey(true);
    }
}