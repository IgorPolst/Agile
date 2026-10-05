using RpgRoguelikeRun.Items.Pricing;
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
    public int DaysInStorage { get; set; }
    public bool IsPerishable => ShelfLifeDays.HasValue;
    public  bool IsSpoiled => IsPerishable && DaysInStorage >= ShelfLifeDays!.Value; 

    public int CurrentCost
    {
        get
        {
            {
                int price = Pricing.PricePipeline.CalculatePrice(this);
                return Math.Max(1, price);   // 👈 цена не может быть ниже 1
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