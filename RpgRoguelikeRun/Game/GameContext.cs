using RpgRoguelikeRun.Entities;
using RpgRoguelikeRun.Enums;
using RpgRoguelikeRun.Services;
using RpgRoguelikeRun.Services.Random;
using RpgRoguelikeRun.WorldLayer;
using RpgRoguelikeRun.Game.States;

namespace RpgRoguelikeRun.Game;

public class GameContext
{
    public World World { get; private set; } = null!;
    public Trader Trader { get; private set; } = null!;
    public Difficulty Difficulty { get; set; }
    public IGameState CurrentState { get; private set; } = null!;

    public bool IsFinished { get; private set; }

    public GameContext(Difficulty difficulty)
    {
        Difficulty = difficulty;
    }
    public void ChangeState(IGameState newState)
    {
        CurrentState = newState;
        newState.Enter(this);
    }

    public void InitializeWorld(int mapWidth, int mapHeight, int? seed = null)
    {
        if (seed.HasValue)
            GameRandom.SetSeed(seed.Value);
        else
            GameRandom.Reset();

        (World, Trader) = GameSetupFacade.StartNewGame(mapWidth, mapHeight, Difficulty);
    }

    public void Finish()
    {
        IsFinished = true;
    }
}