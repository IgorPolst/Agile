using System.Runtime.ConstrainedExecution;
using RpgRoguelikeRun.WorldLayer;
using RpgRoguelikeRun.Enums;

namespace RpgRoguelikeRun;

public sealed class GameManager
{
    public WorldMap worldMap = new(200, 300);
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
                instance.Init(); 
            }
            return instance;
        } 
    }

    public int MapWidth { get; set; }
    public int MapHeight { get; set; }
    public Difficulty Difficulty { get; set; }

    private WorldMap _worldMap = null!;
    private bool _gameStopped;

    private GameManager()
    {
        MapWidth = 80;
        MapHeight = 25;
        Difficulty = Difficulty.Normal;
    } 
    private void Init()
    {
        Console.CursorVisible = false;
        _worldMap = new WorldMap(MapWidth, MapHeight);
        _gameStopped = false;
    }

     
    public void Run()
    {
        Init();

        Console.Clear();
        Console.WriteLine($"Game Started with difficulty: {Difficulty}");
        Console.WriteLine($"Map size: {MapWidth}x{MapHeight}");
        Console.WriteLine("Press any key to start...");
        Console.ReadKey(true);

        while (!_gameStopped)
        {
            HandleInput();
            Update();
            Render();
        }

        Console.CursorVisible = true;
        Console.WriteLine("Game over. Press any key to exit.");
        Console.ReadKey(true);
    }

    private bool GameIsEnded()
    {
        return gameStopped;
    }

    private void HandleInput()
    {
        var key = Console.ReadKey(true);

        switch (key.Key)
        {
            case ConsoleKey.Escape:
                _gameStopped = true;
                break;

            case ConsoleKey.UpArrow:
            case ConsoleKey.W:
            case ConsoleKey.DownArrow:
            case ConsoleKey.S:
            case ConsoleKey.LeftArrow:
            case ConsoleKey.A:
            case ConsoleKey.RightArrow:
            case ConsoleKey.D:
                break;
        }
    }

    private void Update()
    {
        
    }

    private void Render()
    {
        Console.Clear();
        Console.WriteLine($"=== Medieval Trader | {Difficulty} | {MapWidth}x{MapHeight} ===");
        Console.WriteLine();
        Console.WriteLine("WASD / Arrows — move, Escape — exit");
    }

    private bool gameStopped = false;
    private static GameManager? instance;
}