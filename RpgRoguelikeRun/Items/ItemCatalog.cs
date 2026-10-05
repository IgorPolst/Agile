using RpgRoguelikeRun.Services.Random;

namespace RpgRoguelikeRun.Items;

public static class ItemCatalog
{
    private static readonly Dictionary<string, Item> _prototypes = new();

    static ItemCatalog()
    {
        _prototypes["grain"] = new ItemBuilder()
            .WithName("Зерно")
            .WithCategory(ItemCategory.Raw)
            .WithBaseCost(3)
            .WithShelfLife(30)
            .Build();

        _prototypes["salt"] = new ItemBuilder()
            .WithName("Соль")
            .WithCategory(ItemCategory.Raw)
            .WithBaseCost(5)
            .WithoutShelfLife()
            .Build();

        _prototypes["silk"] = new ItemBuilder()
            .WithName("Шёлк")
            .WithCategory(ItemCategory.Luxury)
            .WithBaseCost(25)
            .WithoutShelfLife()
            .Build();

        _prototypes["spice"] = new ItemBuilder()
            .WithName("Пряности")
            .WithCategory(ItemCategory.Luxury)
            .WithBaseCost(15)
            .WithShelfLife(60)
            .Build();

        _prototypes["wine"] = new ItemBuilder()
            .WithName("Вино")
            .WithCategory(ItemCategory.Luxury)
            .WithBaseCost(12)
            .WithShelfLife(90)
            .Build();

        _prototypes["horse"] = new ItemBuilder()
            .WithName("Лошадь")
            .WithCategory(ItemCategory.Livestock)
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
        clone.Rarity = RollRarity();  
        return clone;
    }
    private static ItemRarity RollRarity()
    {
        double roll = GameRandom.NextDouble();
        return roll switch
        {
            < 0.70 => ItemRarity.Common,
            < 0.90 => ItemRarity.Uncommon,
            < 0.98 => ItemRarity.Rare,
            _      => ItemRarity.Legendary
        };
    }

        private static ItemQuality RollQuality()
    {
        double roll = GameRandom.NextDouble();
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