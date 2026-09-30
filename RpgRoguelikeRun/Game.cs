using System.Runtime.ConstrainedExecution;
using RpgRoguelikeRun.WorldLayer;

namespace RpgRoguelikeRun;

public class Game
{
    public WorldMap worldMap = new(200, 300);
    public static Game Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Game();
                instance.Init(); 
            }
            return instance;
        } 
     } 
    private void Init()
    {
        Console.Clear();
        Console.WriteLine("Game is running...");
    }

     
    public void Run()
    {
        Init();
        while (!GameIsEnded())
        {
            HandleInput();
            Update();
            Render();
        }
    }

    private bool GameIsEnded()
    {
        return gameStopped;
    }

    private void HandleInput()
    {
        var key = Console.ReadKey(true);
        if (key.Key == ConsoleKey.Escape)
            gameStopped = true;
    }

    private void Update()
    {
        
    }

    private void Render()
    {
        Console.Clear();
        Console.WriteLine("Game is running.... Press Escape to stop ");
    }

    private bool gameStopped = false;
    private static Game? instance;
}