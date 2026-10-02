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
            // 1. Базовая цена = BaseCost × quality
            double qualityMultiplier = Quality switch
            {
                ItemQuality.Poor      => 0.7,
                ItemQuality.Common    => 1.0,
                ItemQuality.Good      => 1.4,
                ItemQuality.Excellent => 2.0,
                _                     => 1.0
            };

            double basePrice = BaseCost * qualityMultiplier;

            // 2. Скидка за свежесть
            double freshnessMultiplier = 1.0;

            if (IsPerishable)
            {
                double usedFraction = (double)DaysInStorage / ShelfLifeDays!.Value;

                freshnessMultiplier = usedFraction switch
                {
                    < 0.5  => 1.0,    // свежий — без скидки
                    < 0.8  => 0.85,   // лежалый — минус 15%
                    < 1.0  => 0.60,   // скоро испортится — минус 40%
                    _      => 0.25    // испорчен — минус 75%
                };
            }

            return Math.Max(1, (int)Math.Round(basePrice * freshnessMultiplier));
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