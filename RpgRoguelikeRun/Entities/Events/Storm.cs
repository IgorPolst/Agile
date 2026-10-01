using RpgRoguelikeRun.Entities;

namespace RpgRoguelikeRun.Entities.Events;

public class Storm : RoadEvent
{
    public override string Title => "Шторм на дороге";
    public override bool TriggersStormDamage => true;

    public int DelayDays { get; init; } = 1;

    public override void Trigger(Trader trader)
    {
        int actualDelay = Random.Shared.Next(1, DelayDays + 2);
        trader.Delay(actualDelay);
        Console.WriteLine($"🌧️  {Title}: караван застрял на {actualDelay} ход(ов).");
    }
}