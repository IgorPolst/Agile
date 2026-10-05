using RpgRoguelikeRun.Services.Random;

namespace RpgRoguelikeRun.Entities.Events;

public class SunnyDay : RoadEvent
{
    public override string Title => "Солнечный день";
    public override bool IsFriendly => true;

    public override void Trigger(Trader trader)
    {
        Console.WriteLine($"☀️  {Title}: хорошая погода, идти легко!");
        trader.ApplyBonusSteps(1);   // +1 шаг (всего 2 за ход)
    }
}