using RpgRoguelikeRun.Entities;

namespace RpgRoguelikeRun.Entities.Events;

public class HelpfulMerchant : RoadEvent
{
    public override string Title => "Попутный купец";

    public int Bonus { get; init; } = 25;
    public double BonusVariance { get; init; } = 0.4;
    public override bool IsFriendly => true;

    public override void Trigger(Trader trader)
        {
            double min = Bonus * (1 - BonusVariance);
            double max = Bonus * (1 + BonusVariance);
            int actualBonus = (int)Math.Round(min + Random.Shared.NextDouble() * (max - min));

            trader.AddGold(actualBonus);
            Console.WriteLine($"🤝 {Title}: поделился прибылью, +{actualBonus} золота. Теперь: {trader.Gold}");
        }
}