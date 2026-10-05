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

    

    public override string ToString()
        => $"{Name} [{Category}/{Rarity}] cost={CurrentCost} q={Quality}" +
           (IsPerishable ? $" shelf={ShelfLifeDays}d, stored={DaysInStorage}d" : "");
}