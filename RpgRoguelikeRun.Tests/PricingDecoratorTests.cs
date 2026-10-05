using RpgRoguelikeRun.Items;
using RpgRoguelikeRun.Items.Pricing;
using RpgRoguelikeRun.Enums;
using Xunit;

namespace RpgRoguelikeRun.Tests;

public class PricingDecoratorTests
{
    private static Item MakeItem(
        int baseCost = 10,
        ItemQuality quality = ItemQuality.Common,
        ItemRarity rarity = ItemRarity.Common,
        int? shelfLife = null)
        => new Item("Test", ItemCategory.Raw, rarity, baseCost, quality, shelfLife);

    // ================================================================
    // 1. Базовая цена — BaseCost
    // ================================================================
    [Fact]
    public void BasePrice_ReturnsBaseCost()
    {
        var item = MakeItem(baseCost: 42);
        IPriceModifier chain = new BasePrice();

        int price = chain.GetPrice(item);

        Assert.Equal(42, price);
    }

    // ================================================================
    // 2. Quality — Excellent удваивает
    // ================================================================
    [Fact]
    public void QualityDecorator_Excellent_DoublesPrice()
    {
        var item = MakeItem(baseCost: 10, quality: ItemQuality.Excellent);
        IPriceModifier chain = new QualityDecorator(new BasePrice());

        int price = chain.GetPrice(item);

        Assert.Equal(20, price);
    }

    // ================================================================
    // 3. Цепочка: Quality + Rarity
    // ================================================================
    [Fact]
    public void Chain_QualityAndRarity_MultipliesBoth()
    {

        var item = MakeItem(baseCost: 10, quality: ItemQuality.Good, rarity: ItemRarity.Rare);
        // 10 * 1.4 * 1.4 = 19.6 → 20
        IPriceModifier chain = new RarityDecorator(new QualityDecorator(new BasePrice()));

        int price = chain.GetPrice(item);

        Assert.Equal(20, price);
    }

    // ================================================================
    // 4. Цепочка из 3: Quality + Rarity + Freshness
    // ================================================================
    [Fact]
    public void Chain_ThreeDecorators_CorrectOrder()
    {

        var item = MakeItem(baseCost: 10, quality: ItemQuality.Common,
                            rarity: ItemRarity.Common, shelfLife: 30);
        item.DaysInStorage = 20;   // 20/30 = 0.66 → freshness 0.85

        IPriceModifier chain = new FreshnessDecorator(
            new RarityDecorator(
                new QualityDecorator(
                    new BasePrice())));

        int price = chain.GetPrice(item);

        Assert.Equal(9, price);
    }

    // ================================================================
    // 5. Непортящийся товар — Freshness не влияет
    // ================================================================
    [Fact]
    public void FreshnessDecorator_NonPerishable_DoesNotChangePrice()
    {
        var item = MakeItem(baseCost: 10, shelfLife: null);
        item.DaysInStorage = 9999;
        IPriceModifier chain = new FreshnessDecorator(new BasePrice());

        int price = chain.GetPrice(item);

        Assert.Equal(10, price);
    }

    // ================================================================
    // 6. Порядок декораторов влияет на результат
    // ================================================================
    [Fact]
    public void OrderOfDecorators_AffectsResult()
    {

        var item = MakeItem(baseCost: 10, quality: ItemQuality.Excellent);

        var chainA = new QualityDecorator(new BasePrice());

        int priceA = chainA.GetPrice(item);

        Assert.Equal(20, priceA);
    }
}