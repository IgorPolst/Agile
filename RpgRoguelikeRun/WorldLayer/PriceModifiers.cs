using RpgRoguelikeRun.Items;

namespace RpgRoguelikeRun.WorldLayer;

public static class PriceModifiers
{
    public static double GetMultiplier(LocationType type, ItemCategory category)
        => (type, category) switch
        {
            // Деревня: сырьё дёшево, роскошь дорого
            (LocationType.Village, ItemCategory.Raw)       => 0.65,
            (LocationType.Village, ItemCategory.Livestock) => 0.75,
            (LocationType.Village, ItemCategory.Luxury)    => 1.35,
            (LocationType.Village, ItemCategory.Crafted)   => 1.10,

            // Порт: роскошь дёшево, сырьё дорого
            (LocationType.Port, ItemCategory.Luxury)       => 0.65,
            (LocationType.Port, ItemCategory.Raw)          => 1.30,
            (LocationType.Port, ItemCategory.Crafted)      => 1.00,
            (LocationType.Port, ItemCategory.Livestock)    => 1.20,

            // Шахта: сырьё дёшево, роскошь дорого
            (LocationType.Mine, ItemCategory.Raw)          => 0.60,
            (LocationType.Mine, ItemCategory.Crafted)      => 1.25,
            (LocationType.Mine, ItemCategory.Luxury)       => 1.40,

            // Монастырь: роскошь (вино, книги) дёшево
            (LocationType.Monastery, ItemCategory.Luxury)  => 0.80,
            (LocationType.Monastery, ItemCategory.Raw)     => 1.15,

            // Город: ремесло дёшево, сырьё дорого
            (LocationType.Town, ItemCategory.Crafted)      => 0.75,
            (LocationType.Town, ItemCategory.Raw)          => 1.20,

            _ => 1.0
        };
}