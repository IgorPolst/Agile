using RpgRoguelikeRun.Items;
using RpgRoguelikeRun.Services.Random;

namespace RpgRoguelikeRun.Entities.Events;

public class HelpfulMerchant : RoadEvent
{
    public override string Title => "Попутный купец";
    public override bool IsFriendly => true;
    public int Bonus { get; init; } = 25;
    public double BonusVariance { get; init; } = 0.4;
    public double GiftChance { get; init; } = 0.5;
    public override void Trigger(Trader trader)
    {
        // 1. Золото — всегда
        double min = Bonus * (1 - BonusVariance);
        double max = Bonus * (1 + BonusVariance);
        int actualBonus = (int)Math.Round(min + GameRandom.NextDouble() * (max - min));

        Item? gift = null;
        if (GameRandom.NextDouble() < GiftChance)
        {
            gift = PickRandomGift();
        }

        Console.WriteLine($"🤝 {Title}:");

        if (gift != null)
        {
            if (trader.Inventory.HasSpaceFor(1))
            {
                trader.Inventory.Add(gift, 1);
                Console.WriteLine($"   🎁 Подарил: {gift.Name} (базовая цена {gift.BaseCost})");
            }
            else
            {
                Console.WriteLine("   ⚠️  У вас нет места для подарка — только золото.");
            }
        }

        trader.AddGold(actualBonus);
        Console.WriteLine($"   💰 Дал {actualBonus} золота. Теперь: {trader.Gold}");
    }

    private static Item PickRandomGift()
    {
        string[] gifts = { "grain", "salt", "wine", "spice" };
        string key = gifts[GameRandom.Next(0, gifts.Length)];
        return ItemCatalog.Create(key);
    }
}