using RpgRoguelikeRun.Enums;

namespace RpgRoguelikeRun.Items;


public class Item : ICloneable
{
    // ---------- Идентификация ----------
    public string Name { get; set; }
    public ItemCategory Category { get; set; }
    public ItemRarity Rarity { get; set; }

    // ---------- Характеристики ----------
    public int BaseCost { get; set; }
    public ItemQuality Quality { get; set; }
    public int? ShelfLifeDays { get; set; }
    private int _daysInStorage;

    public int DaysInStorage
    {
        get => _daysInStorage;
        set
        {
            if (value == _daysInStorage) return;   // 👈 не дёргаем событие зря

            bool wasSpoiled = IsSpoiled;
            _daysInStorage = value;
            bool isSpoiled = IsSpoiled;

            // Если товар перешёл из «свежего» в «испорченный» — кричим
            if (!wasSpoiled && isSpoiled)
                OnSpoiled?.Invoke(this);
        }
    }
    // ---------- Observer ----------

    public event Action<Item>? OnSpoiled;
    public bool IsPerishable => ShelfLifeDays.HasValue;
    public  bool IsSpoiled => IsPerishable && DaysInStorage >= ShelfLifeDays!.Value; 

    

    public int CurrentCost
    {
        get
        {
            {
                int price = Pricing.PricePipeline.CalculatePrice(this);
                return Math.Max(1, price);
            }
        }
    }

    public Item(string name,
                ItemCategory category,
                ItemRarity rarity,
                int baseCost,
                ItemQuality quality = ItemQuality.Common,
                int? shelfLifeDays = null)
    {
        Name = name;
        Category = category;
        Rarity = rarity;
        BaseCost = baseCost;
        Quality = quality;
        ShelfLifeDays = shelfLifeDays;
    }

    // ---------- Prototype ----------

    public object Clone()
    {
        return new Item(Name, Category, Rarity, BaseCost, Quality, ShelfLifeDays)
        {
            DaysInStorage = DaysInStorage
        };
    }

    public string GetPublicDescription()
    {
        return $"{Name} [{Quality}, {Rarity}]";
    }

    public string GetFullDescription()
    {
        var parts = new List<string>
        {
            $"{Name}",
            $"Категория: {Category}",
            $"Редкость: {Rarity}",
            $"Качество: {Quality}",
            $"Цена: {CurrentCost}"
        };

        if (IsPerishable)
        {
            parts.Add($"Срок годности: {ShelfLifeDays} дней");
            parts.Add($"Пролежал: {DaysInStorage} дней");
            parts.Add(IsSpoiled ? "⚠️ ИСПОРЧЕН" : "✓ Свежий");
        }
        else
        {
            parts.Add("Не портится");
        }

        return string.Join(" | ", parts);
    }

    public string GetBuyDescription()
    {
        int mask = Services.Random.GameRandom.Next(0, 4);

        bool showQuality = (mask & 1) != 0;
        bool showRarity = (mask & 2) != 0;

        if (!showQuality && !showRarity)
            return $"{Name}";

        var parts = new List<string> { Name };

        if (showQuality) parts.Add($"Качество: {Quality}");
        if (showRarity)  parts.Add($"Редкость: {Rarity}");

        return string.Join(" | ", parts);
    }
    

    public override string ToString()
        => GetFullDescription();
}