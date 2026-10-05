using RpgRoguelikeRun.Items;
using RpgRoguelikeRun.Services.Random;
using RpgRoguelikeRun.WorldLayer.Distribution;

namespace RpgRoguelikeRun.WorldLayer;

public static class MarketFactory
{
    private const double DefaultVariance = 0.15;

    // ---------- Публичный API ----------

    public static Market CreateVillageMarket()   => CreateMarketFor(LocationType.Village);
    public static Market CreatePortMarket()      => CreateMarketFor(LocationType.Port);
    public static Market CreateMineMarket()      => CreateMarketFor(LocationType.Mine);
    public static Market CreateMonasteryMarket() => CreateMarketFor(LocationType.Monastery);
    public static Market CreateTownMarket()      => CreateMarketFor(LocationType.Town);

    // ---------- Процедурная генерация ----------
        private static Market CreateMarketFor(LocationType type)
    {
        var market = new Market();
        var categories = GetCategoriesFor(type);
        int lotCount = GameRandom.Next(3, 6);
        var usedKeys = new HashSet<string>();

        int attempts = 0;
        while (market.Lots.Count < lotCount && attempts < 30)
        {
            attempts++;
            var category = categories[GameRandom.Next(0, categories.Count)];

            string? key = PickWeightedKey(category, usedKeys);
            if (key == null) continue;

            Item item = ItemCatalog.Create(key);
            usedKeys.Add(key);

            int qty = MarketDistribution.RollQuantity(item);

            AddWithNoise(market, type, item, qty);
        }

        return market;
    }

    private static List<ItemCategory> GetCategoriesFor(LocationType type) => type switch
    {
        LocationType.Village   => new() { ItemCategory.Raw, ItemCategory.Livestock, ItemCategory.Crafted },
        LocationType.Town      => new() { ItemCategory.Crafted, ItemCategory.Luxury, ItemCategory.Raw },
        LocationType.Port      => new() { ItemCategory.Luxury, ItemCategory.Raw, ItemCategory.Contraband },
        LocationType.Mine      => new() { ItemCategory.Raw, ItemCategory.Crafted },
        LocationType.Monastery => new() { ItemCategory.Luxury, ItemCategory.Crafted },
        _                      => new() { ItemCategory.Raw }
    };

    private static string? PickWeightedKey(ItemCategory category, HashSet<string> excluded)
    {
        var candidates = ItemCatalog.Keys
            .Where(k => !excluded.Contains(k))
            .ToList();

        var weighted = new List<(string key, double weight)>();
        double totalWeight = 0;

        foreach (var key in candidates)
        {
            Item probe = ItemCatalog.Create(key);
            if (probe.Category != category) continue;

            double weight = MarketDistribution.GetWeight(probe);
            weighted.Add((key, weight));
            totalWeight += weight;
        }

        if (weighted.Count == 0) return null;

        double roll = GameRandom.NextDouble() * totalWeight;
        double cumulative = 0;

        foreach (var (key, weight) in weighted)
        {
            cumulative += weight;
            if (roll <= cumulative) return key;
        }

        return weighted[^1].key;
    }

    // ---------- Хелпер с модификаторами ----------

    private static void AddWithNoise(
        Market market,
        LocationType locationType,
        Item item,
        int qty,
        double variance = DefaultVariance)
    {
        int itemPrice = item.CurrentCost;
        double typeMultiplier = PriceModifiers.GetMultiplier(locationType, item.Category);
        double noise = 1.0 + (GameRandom.NextDouble() * 2 - 1) * variance;
        int actualPrice = Math.Max(1, (int)Math.Round(itemPrice * typeMultiplier * noise));

        market.AddLot(item, qty, actualPrice);
    }
}