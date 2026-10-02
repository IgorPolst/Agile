namespace RpgRoguelikeRun.Items;

public static class ItemCatalog
{
    private static readonly Dictionary<string, Item> _prototypes = new();
    private static readonly Random _random = new();

    static ItemCatalog()
    {
        _prototypes["grain"] = new ItemBuilder()
            .WithName("Зерно")
            .WithCategory(ItemCategory.Raw)
            .WithRarity(ItemRarity.Common)
            .WithBaseCost(3)
            .WithQuality(ItemQuality.Common)
            .WithShelfLife(30)
            .Build();

        _prototypes["salt"] = new ItemBuilder()
            .WithName("Соль")
            .WithCategory(ItemCategory.Raw)
            .WithRarity(ItemRarity.Common)
            .WithBaseCost(5)
            .WithoutShelfLife()
            .Build();

        _prototypes["silk"] = new ItemBuilder()
            .WithName("Шёлк")
            .WithCategory(ItemCategory.Luxury)
            .WithRarity(ItemRarity.Rare)
            .WithBaseCost(25)
            .WithQuality(ItemQuality.Good)
            .WithoutShelfLife()
            .Build();

        _prototypes["spice"] = new ItemBuilder()
            .WithName("Пряности")
            .WithCategory(ItemCategory.Luxury)
            .WithRarity(ItemRarity.Uncommon)
            .WithBaseCost(15)
            .WithQuality(ItemQuality.Good)
            .WithShelfLife(60)
            .Build();

        _prototypes["wine"] = new ItemBuilder()
            .WithName("Вино")
            .WithCategory(ItemCategory.Luxury)
            .WithRarity(ItemRarity.Uncommon)
            .WithBaseCost(12)
            .WithShelfLife(90)
            .Build();

        _prototypes["horse"] = new ItemBuilder()
            .WithName("Лошадь")
            .WithCategory(ItemCategory.Livestock)
            .WithRarity(ItemRarity.Uncommon)
            .WithBaseCost(80)
            .WithoutShelfLife()
            .Build();
    }

    public static Item Create(string key)
    {
        if (!_prototypes.TryGetValue(key, out var prototype))
            throw new ArgumentException($"Неизвестный товар: {key}");

        Item clone = (Item)prototype.Clone();
        clone.Quality = RollQuality();
        return clone;
    }

        private static ItemQuality RollQuality()
    {
        double roll = _random.NextDouble();
        return roll switch
        {
            < 0.10 => ItemQuality.Poor,
            < 0.70 => ItemQuality.Common,
            < 0.95 => ItemQuality.Good,
            _      => ItemQuality.Excellent
        };
    }

    public static IEnumerable<string> Keys => _prototypes.Keys;
}