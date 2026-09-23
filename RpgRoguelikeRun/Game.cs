using System.Runtime.ConstrainedExecution;

namespace RpgRoguelikeRun;

public class Game
{
    public void Init()
    {
        Console.Clear();
        Console.WriteLine("Game is running...");
    }

     
    public void Run()
    {
        while (!GameIsEnded())
        {
            Init();
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
}