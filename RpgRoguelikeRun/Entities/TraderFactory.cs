using RpgRoguelikeRun.Items;
using RpgRoguelikeRun.Enums;

namespace RpgRoguelikeRun.Entities;

public static class TraderFactory
{
    private const int StartingGoodsValue = 50;
    private static readonly Random _random = new();
    public static Trader CreateStartingTrader(string name = "Ганс", int gold = 100)
    {
        var trader = new Trader(name, gold: 100);
        GiveRandomGoods(trader, StartingGoodsValue);
        return trader;
    }

    public static void GiveRandomGoods(Trader trader, int budget)
    {
        var allKeys = ItemCatalog.Keys.ToList();
        int remaining = budget;

        while (remaining > 0)
        {
            string key = allKeys[_random.Next(allKeys.Count)];
            Item item = ItemCatalog.Create(key);

            int price = item.BaseCost;
            if (price > remaining) break;

            trader.Inventory.Add(item);
            remaining -= price;
        }
    }
}