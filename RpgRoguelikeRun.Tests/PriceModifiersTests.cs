using RpgRoguelikeRun.Items;
using RpgRoguelikeRun.Enums;
using RpgRoguelikeRun.WorldLayer;
using Xunit;

namespace RpgRoguelikeRun.Tests;

public class PriceModifiersTests
{
    // ================================================================
    // 1. В деревне сырьё дешевле
    // ================================================================
    [Fact]
    public void GetMultiplier_VillageRaw_IsLessThanOne()
    {
        double multiplier = PriceModifiers.GetMultiplier(LocationType.Village, ItemCategory.Raw);

        Assert.True(multiplier < 1.0);
        Assert.Equal(0.65, multiplier, precision: 2);
    }

    // ================================================================
    // 2. В порту роскошь дешевле
    // ================================================================
    [Fact]
    public void GetMultiplier_PortLuxury_IsLessThanOne()
    {
        double multiplier = PriceModifiers.GetMultiplier(LocationType.Port, ItemCategory.Luxury);

        Assert.Equal(0.65, multiplier, precision: 2);
    }

    // ================================================================
    // 3. В деревне роскошь дороже
    // ================================================================
    [Fact]
    public void GetMultiplier_VillageLuxury_IsGreaterThanOne()
    {
        double multiplier = PriceModifiers.GetMultiplier(LocationType.Village, ItemCategory.Luxury);

        Assert.True(multiplier > 1.0);
    }

    // ================================================================
    // 4. ГРАНИЧНЫЙ: неизвестное сочетание → 1.0
    // ================================================================
    [Fact]
    public void GetMultiplier_UnknownCombination_ReturnsOne()
    {
        double multiplier = PriceModifiers.GetMultiplier(LocationType.Town, ItemCategory.Livestock);

        Assert.Equal(1.0, multiplier, precision: 2);
    }
}