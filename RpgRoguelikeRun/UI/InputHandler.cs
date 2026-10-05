namespace RpgRoguelikeRun.UI;

public static class InputHandler
{
    public static GameCommand ReadCommand()
    {
        var key = Console.ReadKey(true).Key;

        return key switch
        {
            ConsoleKey.Escape => GameCommand.Exit,
            ConsoleKey.RightArrow or ConsoleKey.D => GameCommand.Step,
            ConsoleKey.B => GameCommand.OpenMarket,
            ConsoleKey.T => GameCommand.Travel,

            _ => GameCommand.None
        };
    }
}

public enum GameCommand
{
    None,
    Step,
    OpenMarket,
    Travel,
    Exit
}