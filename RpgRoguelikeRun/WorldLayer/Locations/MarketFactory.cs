using RpgRoguelikeRun.Enums;
using RpgRoguelikeRun.Items;
using RpgRoguelikeRun.Services.Random;
using RpgRoguelikeRun.WorldLayer.Regions;


namespace RpgRoguelikeRun.WorldLayer;

public static class MarketFactory
{
    private const double DefaultVariance = 0.15;
    

    // ---------- Публичный API ----------

    public static Market CreateVillageMarket(IRegionStrategy region)   => CreateMarketFor(LocationType.Village, region);
    public static Market CreatePortMarket(IRegionStrategy region)      => CreateMarketFor(LocationType.Port, region);
    public static Market CreateMineMarket(IRegionStrategy region)      => CreateMarketFor(LocationType.Mine, region);
    public static Market CreateMonasteryMarket(IRegionStrategy region) => CreateMarketFor(LocationType.Monastery, region);
    public static Market CreateTownMarket(IRegionStrategy region)      => CreateMarketFor(LocationType.Town, region);
    
    // ---------- Процедурная генерация ----------
    
    private static Market CreateMarketFor(LocationType type, IRegionStrategy region)
    {
        var market = new Market();
        var categories = region.GetCategories();
        int lotCount = GameRandom.Next(MinLotsPerMarket, MaxLotsPerMarket + 1);
        var usedKeys = new HashSet<string>();

        int attempts = 0;
        while (market.Lots.Count < lotCount && attempts < MaxGenerationAttempts)
        {
            attempts++;
            var category = categories[GameRandom.Next(0, categories.Count)];

            string? key = PickWeightedKey(category, usedKeys, region);
            if (key == null) continue;

            Item item = ItemCatalog.Create(key);
            item.Quality = ApplyQualityShift(item.Quality, region.GetQualityShift(item));
            usedKeys.Add(key);

            int qty = MarketDistribution.RollQuantity(item);

            AddWithNoise(market, type, region, item, qty);
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

    private static string? PickWeightedKey(ItemCategory category, HashSet<string> excluded, IRegionStrategy region)
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

            double weight = MarketDistribution.GetWeight(probe) * region.GetSpawnWeight(probe);
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
        IRegionStrategy region,
        Item item,
        int qty,
        double variance = DefaultVariance)
    {
        int itemPrice = item.CurrentCost;

        // 👇 цена = локальный множитель × региональный × шум
        double typeMultiplier = PriceModifiers.GetMultiplier(locationType, item.Category);
        double regionMultiplier = region.GetPriceMultiplier(item);
        double noise = 1.0 + (GameRandom.NextDouble() * 2 - 1) * variance;

        int actualPrice = Math.Max(1, (int)Math.Round(
            itemPrice * typeMultiplier * regionMultiplier * noise));

        market.AddLot(item, qty, actualPrice);
    }

    private static ItemQuality ApplyQualityShift(ItemQuality quality, int shift)
    {
        int value = (int)quality + shift;
        value = Math.Clamp(value, 0, Enum.GetValues<ItemQuality>().Length - 1);
        return (ItemQuality)value;
    }

    private const int MinLotsPerMarket = 3;
    private const int MaxLotsPerMarket = 5;
    private const int MaxGenerationAttempts = 30;

}