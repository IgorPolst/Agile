using RpgRoguelikeRun.Items;

namespace RpgRoguelikeRun.Entities;

public static class TraderFactory
{
    public static Trader CreateStartingTrader(string name = "Ганс", int gold = 100)
    {
        var trader = new Trader(name, gold);
        FillStartingInventory(trader.Inventory);
        return trader;
    }

    private static void FillStartingInventory(Inventory inventory)
    {
        var kit = new (string key, int qty)[]
        {
            ("grain", 5),
            ("salt",  3),
            ("silk",  1),
            ("spice", 2),
        };

        foreach (var (key, qty) in kit)
            inventory.Add(ItemCatalog.Create(key), qty);
    }
}