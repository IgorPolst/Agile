using RpgRoguelikeRun.Items;

namespace RpgRoguelikeRun.WorldLayer;

public static class MarketFactory
{
    private static readonly Random _random = new();

    public static Market CreateVillageMarket()
    {
        var m = new Market();
        AddWithNoise(m, LocationType.Village, "grain", 30, 3);
        AddWithNoise(m, LocationType.Village, "salt",  5,  7);
        return m;
    }

    public static Market CreatePortMarket()
    {
        var m = new Market();
        AddWithNoise(m, LocationType.Port, "silk",  5,  20);
        AddWithNoise(m, LocationType.Port, "spice", 8,  12);
        AddWithNoise(m, LocationType.Port, "grain", 15, 5);
        return m;
    }

    public static Market CreateMineMarket()
    {
        var m = new Market();
        AddWithNoise(m, LocationType.Mine, "salt",  20, 5);
        AddWithNoise(m, LocationType.Mine, "grain", 10, 6);
        return m;
    }

    public static Market CreateMonasteryMarket()
    {
        var m = new Market();
        AddWithNoise(m, LocationType.Monastery, "wine",  10, 12);
        AddWithNoise(m, LocationType.Monastery, "spice", 3,  18);
        return m;
    }

    public static Market CreateTownMarket()
    {
        var m = new Market();
        AddWithNoise(m, LocationType.Town, "silk",  3,  25);
        AddWithNoise(m, LocationType.Town, "wine",  5,  15);
        AddWithNoise(m, LocationType.Town, "grain", 12, 4);
        return m;
    }

    private static void AddWithNoise(
        Market market,
        LocationType locationType,
        string itemKey,
        int qty,
        int basePrice,
        double variance = 0.15)
    {
        Item item = ItemCatalog.Create(itemKey);
        double typeMultiplier = PriceModifiers.GetMultiplier(locationType, item.Category);
        double noise = 1.0 + (_random.NextDouble() * 2 - 1) * variance;
        int actualPrice = Math.Max(1, (int)Math.Round(basePrice * typeMultiplier * noise));
        market.AddLot(item, qty, actualPrice);
    }
}