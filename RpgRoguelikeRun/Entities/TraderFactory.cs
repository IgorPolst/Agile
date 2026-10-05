using RpgRoguelikeRun.Items;
using RpgRoguelikeRun.Services.Random;

namespace RpgRoguelikeRun.Entities;

public static class TraderFactory
{
    private const int StartingGoodsValue = 50;
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

        allKeys.Sort((a, b) =>
            ItemCatalog.Create(a).BaseCost.CompareTo(ItemCatalog.Create(b).BaseCost));

        int attempts = 0;
        while (remaining > 0 && attempts < 100)
        {
            attempts++;
            var affordable = allKeys
                .Where(k => ItemCatalog.Create(k).BaseCost <= remaining)
                .ToList();

            if (affordable.Count == 0) break;

            string key = affordable[GameRandom.Next(0, affordable.Count)];
            Item item = ItemCatalog.Create(key);

            trader.Inventory.Add(item);
            remaining -= item.BaseCost;
        }

        if (trader.Inventory.Stacks.Count == 0)
        {
            string cheapest = allKeys
                .OrderBy(k => ItemCatalog.Create(k).BaseCost)
                .First();

            trader.Inventory.Add(ItemCatalog.Create(cheapest));
        }
    }
}