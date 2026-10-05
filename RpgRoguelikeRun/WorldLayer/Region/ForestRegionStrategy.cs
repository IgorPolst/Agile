using RpgRoguelikeRun.Items;
using RpgRoguelikeRun.Enums;

namespace RpgRoguelikeRun.WorldLayer.Regions;

public class ForestRegionStrategy : IRegionStrategy
{
    public string Name => "Лесной";

    public List<ItemCategory> GetCategories() => new()
    {
        ItemCategory.Raw,
        ItemCategory.Livestock,
        ItemCategory.Crafted
    };

    public double GetPriceMultiplier(Item item) => item.Category switch
    {
        ItemCategory.Raw       => 0.85,
        ItemCategory.Livestock => 1.20,
        ItemCategory.Crafted   => 0.90,
        ItemCategory.Luxury    => 1.35,
        _                      => 1.0
    };

    public int GetQualityShift(Item item) => item.Name switch
    {
        "Зерно" => +1,
        "Соль" => -1,
        _ => 0
    };

    public double GetSpawnWeight(Item item) => item.Name switch
    {
        "Зерно" => 3.0,
        "Пряности" => 1.5,
        "Соль" => 0.4,
        _ => 1.0
    };
}