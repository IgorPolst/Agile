using RpgRoguelikeRun.Items;
using RpgRoguelikeRun.Enums;
using Xunit;

namespace RpgRoguelikeRun.Tests;

public class ItemBuilderTests
{
    // ================================================================
    // 1. ПОЗИТИВНЫЙ: Builder собирает объект с заданными полями
    // ================================================================
    [Fact]
    public void Build_WithAllParameters_CreatesCorrectItem()
    {
        var item = new ItemBuilder()
            .WithName("Шёлк")
            .WithCategory(ItemCategory.Luxury)
            .WithRarity(ItemRarity.Rare)
            .WithBaseCost(25)
            .WithQuality(ItemQuality.Good)
            .WithShelfLife(60)
            .Build();

        Assert.Equal("Шёлк", item.Name);
        Assert.Equal(ItemCategory.Luxury, item.Category);
        Assert.Equal(ItemRarity.Rare, item.Rarity);
        Assert.Equal(25, item.BaseCost);
        Assert.Equal(ItemQuality.Good, item.Quality);
        Assert.Equal(60, item.ShelfLifeDays);
    }

    // ================================================================
    // 2. ГРАНИЧНЫЙ: Builder без вызовов возвращает дефолт
    // ================================================================
    [Fact]
    public void Build_WithoutConfiguration_ReturnsDefaultItem()
    {
        var item = new ItemBuilder().Build();

        Assert.Equal("Безымянный товар", item.Name);
        Assert.Equal(ItemCategory.Raw, item.Category);
        Assert.Equal(ItemRarity.Common, item.Rarity);
        Assert.Equal(1, item.BaseCost);
    }

    // ================================================================
    // 3. НЕГАТИВНЫЙ: WithBaseCost(-1) бросает исключение
    // ================================================================
    [Fact]
    public void WithBaseCost_Negative_ThrowsException()
    {
        var builder = new ItemBuilder();

        Assert.Throws<ArgumentOutOfRangeException>(() => builder.WithBaseCost(-1));
    }

    // ================================================================
    // 4. Fluent Interface: цепочка возвращает тот же builder
    // ================================================================
    [Fact]
    public void WithName_ReturnsSameBuilderInstance()
    {
        var builder = new ItemBuilder();

        var returned = builder.WithName("X");

        Assert.Same(builder, returned);
    }
}