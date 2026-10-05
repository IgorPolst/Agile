using RpgRoguelikeRun.Enums;
namespace RpgRoguelikeRun.Items;

public class ItemBuilder
{
    private string _name = "Безымянный товар";
    private ItemCategory _category = ItemCategory.Raw;
    private ItemRarity _rarity = ItemRarity.Common;
    private int _baseCost = 1;
    private ItemQuality _quality = ItemQuality.Common;
    private int? _shelfLifeDays = null;

    // ---------- Настройка ----------

    public ItemBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ItemBuilder WithCategory(ItemCategory category)
    {
        _category = category;
        return this;
    }

    public ItemBuilder WithRarity(ItemRarity rarity)
    {
        _rarity = rarity;
        return this;
    }

    public ItemBuilder WithBaseCost(int cost)
    {
        if (cost < 0) throw new ArgumentOutOfRangeException(nameof(cost));
        _baseCost = cost;
        return this;
    }

    public ItemBuilder WithQuality(ItemQuality quality)
    {
        _quality = quality;
        return this;
    }

    public ItemBuilder WithShelfLife(int days)
    {
        if (days <= 0) throw new ArgumentOutOfRangeException(nameof(days));
        _shelfLifeDays = days;
        return this;
    }

    public ItemBuilder WithoutShelfLife()
    {
        _shelfLifeDays = null;
        return this;
    }

    // ---------- Сборка ----------

    public Item Build()
        => new Item(_name, _category, _rarity, _baseCost, _quality, _shelfLifeDays);
}