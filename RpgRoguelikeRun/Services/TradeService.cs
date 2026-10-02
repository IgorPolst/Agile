using RpgRoguelikeRun.Entities;
using RpgRoguelikeRun.Items;
using RpgRoguelikeRun.WorldLayer;

namespace RpgRoguelikeRun.Services;

public static class TradeService
{

    public static string? Buy(Trader trader, Item item, int quantity)
    {
        if (trader.CurrentLocation == null)
            return "Здесь нет рынка.";

        var market = trader.CurrentLocation.Market;
        var lot = market.FindLot(item);

        if (lot == null || lot.Quantity < quantity)
            return "Товара нет на рынке или его не хватает.";

        int pricePerUnit = market.GetBuyPrice(lot);
        int totalPrice = pricePerUnit * quantity;

        if (trader.Gold < totalPrice)
            return $"Не хватает золота: нужно {totalPrice}, есть {trader.Gold}.";

        if (!trader.Inventory.HasSpaceFor(quantity))
            return $"Не хватает места: нужно {quantity}, свободно {trader.Inventory.Capacity - trader.Inventory.Count}.";

        trader.LoseGold(totalPrice);
        trader.Inventory.Add(item, quantity);
        lot.Quantity -= quantity;
        lot.RegisterTrade(quantity);
        if (lot.Quantity == 0) market.Lots.Remove(lot);

        return null;
    }

    public static string? Sell(Trader trader, Item item, int quantity)
    {
        if (trader.CurrentLocation == null)
            return "Здесь нет рынка.";

        int available = trader.Inventory.CountOf(item);
        if (available < quantity)
            return $"Не хватает товара: нужно {quantity}, есть {available}.";

        var market = trader.CurrentLocation.Market;
        int pricePerUnit = market.GetSellPrice(item, quantity);
        int totalPrice = pricePerUnit * quantity;

        trader.Inventory.Remove(item, quantity);
        trader.AddGold(totalPrice);

        var lot = market.FindLot(item);
        if (lot != null)
            lot.RegisterTrade(quantity);
        else
        {
            market.AddLot((Item)item.Clone(), quantity, pricePerUnit);
            market.Lots.Last().RegisterTrade(quantity);
        }

        return null;
    }
}