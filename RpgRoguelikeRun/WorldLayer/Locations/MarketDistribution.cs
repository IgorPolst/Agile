using RpgRoguelikeRun.Items;
using RpgRoguelikeRun.Enums;
using RpgRoguelikeRun.Services.Random;

namespace RpgRoguelikeRun.WorldLayer;

public static class MarketDistribution
{
    private const int MinBaseQuantity = 5;
    private const int MaxBaseQuantity = 20;

    public static int RollQuantity(Item item)
    {
        int baseQty = GameRandom.Next(MinBaseQuantity, MaxBaseQuantity + 1);

        double qualityFactor = item.Quality switch
        {
            ItemQuality.Poor      => 2.0,
            ItemQuality.Common    => 1.0,
            ItemQuality.Good      => 0.6,
            ItemQuality.Excellent => 0.3,
            _                     => 1.0
        };

        double rarityFactor = item.Rarity switch
        {
            ItemRarity.Common    => 1.0,
            ItemRarity.Uncommon  => 0.7,
            ItemRarity.Rare      => 0.4,
            ItemRarity.Legendary => 0.15,
            _                    => 1.0
        };

        int qty = (int)Math.Round(baseQty * qualityFactor * rarityFactor);
        return Math.Max(1, qty);
    }

    public static double GetWeight(Item item)
    {
        double qualityWeight = item.Quality switch
        {
            ItemQuality.Poor      => 3.0,
            ItemQuality.Common    => 2.0,
            ItemQuality.Good      => 1.0,
            ItemQuality.Excellent => 0.3,
            _                     => 1.0
        };

        double rarityWeight = item.Rarity switch
        {
            ItemRarity.Common    => 3.0,
            ItemRarity.Uncommon  => 1.5,
            ItemRarity.Rare      => 0.6,
            ItemRarity.Legendary => 0.1,
            _                    => 1.0
        };

        return qualityWeight * rarityWeight;
    }
}