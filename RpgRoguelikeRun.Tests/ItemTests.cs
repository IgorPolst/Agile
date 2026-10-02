using RpgRoguelikeRun.Items;
using Xunit;

namespace RpgRoguelikeRun.Tests;

public class ItemTests
{
    // ================================================================
    // 1. ПОЗИТИВНЫЙ: обычный товар стоит BaseCost
    // ================================================================
    [Fact]
    public void CurrentCost_CommonQuality_EqualsBaseCost()
    {
        var item = new Item("Соль", ItemCategory.Raw, ItemRarity.Common, baseCost: 10,
                            quality: ItemQuality.Common);

        int cost = item.CurrentCost;

        Assert.Equal(10, cost);
    }

    // ================================================================
    // 2. ГРАНИЧНЫЙ: Excellent качество ×2
    // ================================================================
    [Fact]
    public void CurrentCost_ExcellentQuality_DoublesBaseCost()
    {
        var item = new Item("Соль", ItemCategory.Raw, ItemRarity.Common, baseCost: 10,
                            quality: ItemQuality.Excellent);

        int cost = item.CurrentCost;

        Assert.Equal(20, cost);
    }

    // ================================================================
    // 3. ГРАНИЧНЫЙ: испорченный товар = BaseCost / 4
    // ================================================================
    [Fact]
    public void CurrentCost_SpoiledItem_QuarterOfBaseCost()
    {
        var item = new Item("Зерно", ItemCategory.Raw, ItemRarity.Common, baseCost: 20,
                            quality: ItemQuality.Common, shelfLifeDays: 5);
        item.DaysInStorage = 5;   // == ShelfLife, значит испорчен

        int cost = item.CurrentCost;

        Assert.Equal(5, cost);   // 20 / 4
    }

    // ================================================================
    // 4. ГРАНИЧНЫЙ: не портящийся товар никогда не Spoiled
    // ================================================================
    [Fact]
    public void IsSpoiled_NonPerishableItem_AlwaysFalse()
    {
        var item = new Item("Соль", ItemCategory.Raw, ItemRarity.Common, baseCost: 5,
                            shelfLifeDays: null);
        item.DaysInStorage = 9999;

        bool spoiled = item.IsSpoiled;

        Assert.False(spoiled);
    }

    // ================================================================
    // 5. PROTOTYPE: клон не влияет на оригинал
    // ================================================================
    [Fact]
    public void Clone_ModifyingClone_DoesNotAffectOriginal()
    {
        var original = new Item("Шёлк", ItemCategory.Luxury, ItemRarity.Rare, baseCost: 25);
        original.DaysInStorage = 10;

        var clone = (Item)original.Clone();
        clone.DaysInStorage = 50;

        Assert.Equal(10, original.DaysInStorage);
        Assert.Equal(50, clone.DaysInStorage);
        Assert.NotSame(original, clone);
    }

    // ================================================================
    // 6. НЕГАТИВНЫЙ: невалидный baseCost
    // ================================================================
    [Fact]
    public void Constructor_NegativeBaseCost_IsAllowedByDesign()
    {
        var item = new Item("X", ItemCategory.Raw, ItemRarity.Common, baseCost: -5);

        Assert.Equal(-5, item.BaseCost);
    }
}