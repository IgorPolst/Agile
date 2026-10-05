namespace RpgRoguelikeRun.Game.States;

public interface IGameState
{
    string Name { get; }
    void Enter(GameContext context);

    bool HandleInput(GameContext context);

    void Render(GameContext context);
}