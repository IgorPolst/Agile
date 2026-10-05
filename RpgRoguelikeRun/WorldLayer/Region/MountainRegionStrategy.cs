using RpgRoguelikeRun.Items;
using RpgRoguelikeRun.Enums;

namespace RpgRoguelikeRun.WorldLayer.Regions;

public class MountainRegionStrategy : IRegionStrategy
{
    public string Name => "Горный";

    public List<ItemCategory> GetCategories() => new()
    {
        ItemCategory.Raw,
        ItemCategory.Crafted,
    };

    public double GetPriceMultiplier(Item item) => item.Category switch
    {
        ItemCategory.Raw       => 0.80,
        ItemCategory.Crafted   => 1.10,
        ItemCategory.Livestock => 1.40,
        _                      => 1.0
    };

    public int GetQualityShift(Item item) => item.Name switch
    {
        "Соль" => +1,
        "Зерно" => -1,
        _ => 0
    };

    public double GetSpawnWeight(Item item) => item.Name switch
    {
        "Соль" => 2.5,
        "Зерно" => 0.3,
        "Лошадь" => 0.5,
        _ => 1.0
    };
}