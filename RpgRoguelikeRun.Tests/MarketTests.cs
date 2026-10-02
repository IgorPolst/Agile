using RpgRoguelikeRun.Items;
using RpgRoguelikeRun.WorldLayer;
using Xunit;

namespace RpgRoguelikeRun.Tests;

public class MarketTests
{
    private static Item MakeItem(string name = "Зерно", int cost = 10)
        => new Item(name, ItemCategory.Raw, ItemRarity.Common, cost);

    // ================================================================
    // 1. СПРЕД: цена покупки > цены продажи
    // ================================================================
    [Fact]
    public void GetBuyPrice_AlwaysHigherThanSellPrice()
    {
        var market = new Market { Spread = 0.20 };
        var item = MakeItem();
        market.AddLot(item, quantity: 10, pricePerUnit: 100);
        var lot = market.FindLot(item)!;

        int buyPrice = market.GetBuyPrice(lot);
        int sellPrice = market.GetSellPrice(item, 1);

        Assert.True(buyPrice > sellPrice,
            $"Buy price {buyPrice} should be greater than sell price {sellPrice}");
    }

    // ================================================================
    // 2. ГРАНИЧНЫЙ: минимальный спред — всё равно +1
    // ================================================================
    [Fact]
    public void GetBuyPrice_ZeroSpread_StillAtLeastPlusOne()
    {
        var market = new Market { Spread = 0.0 };
        var item = MakeItem();
        market.AddLot(item, quantity: 10, pricePerUnit: 50);
        var lot = market.FindLot(item)!;

        int buyPrice = market.GetBuyPrice(lot);

        Assert.Equal(51, buyPrice);  
    }

    // ================================================================
    // 3. ДИНАМИКА: чем больше продано — тем ниже цена
    // ================================================================
    [Fact]
    public void GetSellPrice_AfterTrades_PriceDrops()
    {
        var market = new Market();
        var item = MakeItem();
        market.AddLot(item, quantity: 100, pricePerUnit: 100);
        var lot = market.FindLot(item)!;

        int priceBefore = market.GetSellPrice(item, 1);

        lot.RegisterTrade(quantity: 50);   
        int priceAfter = market.GetSellPrice(item, 1);

        Assert.Equal(100, priceBefore);
        Assert.Equal(95, priceAfter);     
    }

    // ================================================================
    // 4. ГРАНИЧНЫЙ: цена не падает ниже 1
    // ================================================================
    [Fact]
    public void GetSellPrice_HugeTrades_PriceNeverBelowOne()
    {
        var market = new Market();
        var item = MakeItem();
        market.AddLot(item, quantity: 100, pricePerUnit: 10);
        var lot = market.FindLot(item)!;

        lot.RegisterTrade(quantity: 10_000);
        int price = market.GetSellPrice(item, 1);

        Assert.Equal(1, price);
    }

    // ================================================================
    // 5. FindLot возвращает null для отсутствующего товара
    // ================================================================
    [Fact]
    public void FindLot_NonExistingItem_ReturnsNull()
    {
        var market = new Market();
        var item = MakeItem();

        var lot = market.FindLot(item);

        Assert.Null(lot);
    }

    // ================================================================
    // 6. ГРАНИЧНЫЙ: товары с разной Rarity — разные лоты
    // ================================================================
    [Fact]
    public void FindLot_DifferentRarity_ReturnsNull()
    {
        var market = new Market();
        var common = new Item("Шёлк", ItemCategory.Luxury, ItemRarity.Common, 10);
        var rare = new Item("Шёлк", ItemCategory.Luxury, ItemRarity.Rare, 10);
        market.AddLot(common, quantity: 5, pricePerUnit: 20);

        var lot = market.FindLot(rare);

        Assert.Null(lot);
    }
}