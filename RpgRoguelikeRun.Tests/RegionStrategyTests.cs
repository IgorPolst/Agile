using RpgRoguelikeRun.Items;
using RpgRoguelikeRun.WorldLayer.Regions;
using RpgRoguelikeRun.Enums;
using Xunit;

namespace RpgRoguelikeRun.Tests;

public class RegionStrategyTests
{
    private static Item MakeFish() =>
        new Item("Рыба", ItemCategory.Raw, ItemRarity.Common, 5);

    private static Item MakeGrain() =>
        new Item("Зерно", ItemCategory.Raw, ItemRarity.Common, 3);

    // ================================================================
    // 1. Прибрежный регион: рыба дешевле
    // ================================================================
    [Fact]
    public void CoastalRegion_FishCheaper()
    {
        
        var region = new CoastalRegionStrategy();
        var fish = MakeFish();

        
        double multiplier = region.GetPriceMultiplier(fish);

        
        Assert.True(multiplier < 1.0);
    }

    // ================================================================
    // 2. Прибрежный регион: рыба чаще
    // ================================================================
    [Fact]
    public void CoastalRegion_FishSpawnWeightHigher()
    {
        
        var region = new CoastalRegionStrategy();
        var fish = MakeFish();

        
        double weight = region.GetSpawnWeight(fish);

        
        Assert.True(weight > 1.0);
    }

    // ================================================================
    // 3. Прибрежный регион: рыба качественнее
    // ================================================================
    [Fact]
    public void CoastalRegion_FishQualityHigher()
    {
        
        var region = new CoastalRegionStrategy();
        var fish = MakeFish();

        
        int shift = region.GetQualityShift(fish);

        
        Assert.True(shift > 0);
    }

    // ================================================================
    // 4. Горный регион: зерно редко
    // ================================================================
    [Fact]
    public void MountainRegion_GrainSpawnWeightLow()
    {
        
        var region = new MountainRegionStrategy();
        var grain = MakeGrain();

        
        double weight = region.GetSpawnWeight(grain);

        
        Assert.True(weight < 1.0);
    }

    // ================================================================
    // 5. Все регионы возвращают категории
    // ================================================================
    [Theory]
    [InlineData("coastal")]
    [InlineData("forest")]
    [InlineData("mountain")]
    public void AllRegions_HaveCategories(string key)
    {
        
        var region = RegionRegistry.Get(key);

        
        var categories = region.GetCategories();

        
        Assert.NotEmpty(categories);
    }

    // ================================================================
    // 6. Реестр знает 3 региона
    // ================================================================
    [Fact]
    public void RegionRegistry_HasThreeRegions()
    {
        
        var keys = RegionRegistry.Keys.ToList();

        
        Assert.Equal(3, keys.Count);
    }
}