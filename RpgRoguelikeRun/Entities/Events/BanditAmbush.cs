using RpgRoguelikeRun.Entities;

namespace RpgRoguelikeRun.Entities.Events;

public class BanditAmbush : RoadEvent
{
    public int Damage { get; init; } = 50;
    public double DamageVariance { get; init; } = 0.3;
    public override string Title => "Засада разбойников";
    public override bool IsHostile => true;

    public override void Trigger(Trader trader)
    {
        double min = Damage * (1 - DamageVariance);
        double max = Damage * (1 + DamageVariance);
        int actualDamage = (int)Math.Round(min + Random.Shared.NextDouble() * (max - min));

        int stolen = trader.LoseGold(actualDamage);
        Console.WriteLine($"⚔️  {Title}: разбойники отняли {stolen} золота. Осталось: {trader.Gold}");
    }
}