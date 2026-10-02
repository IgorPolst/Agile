using RpgRoguelikeRun.Entities;
using RpgRoguelikeRun.Items;

namespace RpgRoguelikeRun.UI;

public static class MarketMenu
{
    public static void Open(Trader trader)
    {
        if (trader.CurrentLocation == null)
        {
            Console.WriteLine("Здесь нет рынка — вы не в локации.");
            Console.ReadKey(true);
            return;
        }

        while (true)
        {
            Console.Clear();
            Console.WriteLine($"=== РЫНОК: {trader.CurrentLocation.Name} ===");
            Console.WriteLine($"Золото: {trader.Gold}");
            Console.WriteLine($"Инвентарь: {trader.Inventory.Count}/{trader.Inventory.Capacity}");
            Console.WriteLine();

            Console.WriteLine("--- Что продаёт рынок ---");
            var lots = trader.CurrentLocation.Market.Lots;
            if (lots.Count == 0)
                Console.WriteLine("  (пусто)");
            else
                for (int i = 0; i < lots.Count; i++)
                {
                    var lot = lots[i];
                    int buyPrice = trader.CurrentLocation.Market.GetBuyPrice(lot);
                    Console.WriteLine($"  [{i + 1}] {lot.Item.Name} x{lot.Quantity} — {buyPrice} золотых");
                }

            Console.WriteLine();
            Console.WriteLine("--- Ваш инвентарь ---");
            var stacks = trader.Inventory.Stacks;
            if (stacks.Count == 0)
                Console.WriteLine("  (пусто)");
            else
                for (int i = 0; i < stacks.Count; i++)
                {
                    var stack = stacks[i];
                    int sellPrice = trader.CurrentLocation.Market.GetSellPrice(stack.Item, stack.Quantity);
                    Console.WriteLine($"  [{i + 1}] {stack.Item.Name} x{stack.Quantity} — {sellPrice} золотых");
                }

            Console.WriteLine();
            Console.WriteLine("--- Последние сделки ---");
            if (trader.TradeHistory.Count == 0)
                Console.WriteLine("  (пока ничего)");
            else
                foreach (var record in trader.TradeHistory)
                    Console.WriteLine($"  {record}");

            Console.WriteLine();
            Console.WriteLine("B — купить, S — продать, Escape — выйти");

            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Escape) return;
            if (key.Key == ConsoleKey.B) BuyLoop(trader);
            else if (key.Key == ConsoleKey.S) SellLoop(trader);
        }
    }

    // ---------- ЦИКЛИЧНАЯ ПОКУПКА ----------
    private static void BuyLoop(Trader trader)
    {
        while (true)
        {
            var lots = trader.CurrentLocation!.Market.Lots;
            if (lots.Count == 0) { Console.WriteLine("Рынок пуст."); Console.ReadKey(true); return; }

            Console.Clear();
            Console.WriteLine("=== КУПИТЬ ===");
            Console.WriteLine($"Золото: {trader.Gold}");
            Console.WriteLine($"Инвентарь: {trader.Inventory.Count}/{trader.Inventory.Capacity}");
            for (int i = 0; i < lots.Count; i++)
            {
                var lot = lots[i];
                int price = trader.CurrentLocation.Market.GetBuyPrice(lot);
                Console.WriteLine($"  [{i + 1}] {lot.Item.Name} x{lot.Quantity} — {price}/шт");
            }
            Console.WriteLine("  [0] Выйти в меню");

            Console.Write("Номер товара: ");
            if (!int.TryParse(Console.ReadLine(), out int choice)) continue;
            if (choice == 0) return;
            if (choice < 1 || choice > lots.Count) continue;

            var chosenLot = lots[choice - 1];
            Console.Write($"Сколько купить? (есть {chosenLot.Quantity}): ");
            if (!int.TryParse(Console.ReadLine(), out int qty)) continue;
            if (qty <= 0 || qty > chosenLot.Quantity) continue;

            trader.Buy(chosenLot.Item, qty);
            Console.WriteLine("Нажмите любую клавишу...");
            Console.ReadKey(true);
        }
    }

    // ---------- ЦИКЛИЧНАЯ ПРОДАЖА ----------
    private static void SellLoop(Trader trader)
    {
        while (true)
        {
            var stacks = trader.Inventory.Stacks;
            if (stacks.Count == 0) { Console.WriteLine("Инвентарь пуст."); Console.ReadKey(true); return; }

            Console.Clear();
            Console.WriteLine("=== ПРОДАТЬ ===");
            Console.WriteLine($"Золото: {trader.Gold}");
            Console.WriteLine($"Инвентарь: {trader.Inventory.Count}/{trader.Inventory.Capacity}");
            Console.WriteLine();
            for (int i = 0; i < stacks.Count; i++)
            {
                var stack = stacks[i];
                int price = trader.CurrentLocation!.Market.GetSellPrice(stack.Item, stack.Quantity);
                Console.WriteLine($"  [{i + 1}] {stack.Item.Name} x{stack.Quantity} — {price}/шт");
            }
            Console.WriteLine("  [0] Выйти в меню");

            Console.Write("Номер товара: ");
            if (!int.TryParse(Console.ReadLine(), out int choice)) continue;
            if (choice == 0) return;
            if (choice < 1 || choice > stacks.Count) continue;

            var chosenStack = stacks[choice - 1];
            Console.Write($"Сколько продать? (есть {chosenStack.Quantity}): ");
            if (!int.TryParse(Console.ReadLine(), out int qty)) continue;
            if (qty <= 0 || qty > chosenStack.Quantity) continue;

            trader.Sell(chosenStack.Item, qty);
            Console.WriteLine("Нажмите любую клавишу...");
            Console.ReadKey(true);
        }
    }
}