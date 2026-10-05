using RpgRoguelikeRun.Enums;
using RpgRoguelikeRun.Items;

namespace RpgRoguelikeRun.WorldLayer.Regions;

public class CoastalRegionStrategy : IRegionStrategy
{
    public string Name => "Прибрежный";

    public List<ItemCategory> GetCategories() => new()
    {
        ItemCategory.Raw,       
        ItemCategory.Livestock,
    };

    public double GetPriceMultiplier(Item item) => item.Category switch
    {
        ItemCategory.Raw       => 0.75,
        ItemCategory.Livestock => 1.10,
        ItemCategory.Luxury    => 1.30,
        ItemCategory.Crafted   => 1.15,
        _                      => 1.0
    };

    public int GetQualityShift(Item item) => item.Name switch
    {
        "Рыба" or "Соль" => +1,
        "Шёлк" or "Пряности" => -1,
        _ => 0
    };

    public double GetSpawnWeight(Item item) => item.Name switch
    {
        "Рыба" => 3.0,
        "Соль" => 2.0,
        "Шёлк" => 0.5,
        "Пряности" => 0.5,
        _ => 1.0
    };
}