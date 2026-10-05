using RpgRoguelikeRun.Enums;
using RpgRoguelikeRun.Items;
using RpgRoguelikeRun.WorldLayer;

namespace RpgRoguelikeRun.WorldLayer.Regions;

public interface IRegionStrategy
{
    string Name { get; }

    List<ItemCategory> GetCategories();
    double GetPriceMultiplier(Item item);
    int GetQualityShift(Item item);
    double GetSpawnWeight(Item item);
}