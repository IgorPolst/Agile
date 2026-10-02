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
    public bool IsSpoiled =>
        ShelfLifeDays.HasValue && DaysInStorage >= ShelfLifeDays.Value;

    public int CurrentCost
    {
        get
        {
            if (IsSpoiled) return Math.Max(1, BaseCost / 4);

            double qualityMultiplier = Quality switch
            {
                ItemQuality.Poor      => 0.7,
                ItemQuality.Common    => 1.0,
                ItemQuality.Good      => 1.4,
                ItemQuality.Excellent => 2.0,
                _                     => 1.0
            };

            double rarityMultiplier = Rarity switch
            {
                ItemRarity.Common    => 1.0,
                ItemRarity.Uncommon  => 1.3,
                ItemRarity.Rare      => 1.8,
                ItemRarity.Legendary => 3.0,
                _                    => 1.0
            };

            return (int)Math.Round(BaseCost * qualityMultiplier * rarityMultiplier);
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