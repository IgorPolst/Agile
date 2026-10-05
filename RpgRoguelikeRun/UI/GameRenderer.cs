using RpgRoguelikeRun.Entities;
using RpgRoguelikeRun.Enums;
using RpgRoguelikeRun.WorldLayer;
using RpgRoguelikeRun.WorldLayer.Roads;

namespace RpgRoguelikeRun.UI;

public static class GameRenderer
{
    public static void Render(
        Trader trader,
        World world,
        Difficulty difficulty,
        int mapWidth,
        int mapHeight)
    {
        Console.Clear();
        Console.WriteLine($"=== Medieval Trader | {difficulty} | {mapWidth}x{mapHeight} ===");
        Console.WriteLine($"Trader: {trader.Name} | Gold: {trader.Gold}");

        RenderInventory(trader);
        RenderLocationOrRoad(trader);

        if (trader.IsDelayed)
            Console.WriteLine($"Задержан на {trader.DaysDelayed} ход(ов)");

        if (trader.LastEvent != null)
            Console.WriteLine($"Last event: {trader.LastEvent.Title}");

        if (trader.LastSpoiledMessage != null){
            Console.WriteLine();
            Console.WriteLine($"⚠️ {trader.LastSpoiledMessage}");
        }

        Console.WriteLine();
        Console.WriteLine("D — идти, B — рынок, T — выйти из локации, Escape — exit");
    }

    private static void RenderInventory(Trader trader)
    {
        Console.WriteLine();
        Console.WriteLine("--- Инвентарь ---");

        if (trader.Inventory.Stacks.Count == 0)
        {
            Console.WriteLine("  (пусто)");
            return;
        }

        foreach (var stack in trader.Inventory.Stacks)
            Console.WriteLine($"  • {stack}");
    }

    private static void RenderLocationOrRoad(Trader trader)
    {
        Console.WriteLine();

        if (trader.CurrentLocation is Location loc)
        {
            Console.WriteLine($"Локация: {loc.Type}, {loc.Name}, регион: {loc.Region.Name}");
            Console.WriteLine($"   Рынок: {loc.Market.Lots.Count} лотов");
            Console.WriteLine($"   Дорог отсюда: {loc.OutgoingRoads.Count}");

            foreach (var r in loc.OutgoingRoads)
                Console.WriteLine($"      → {r.Destination?.Name ?? "?"} | {r.TravelTime} ходов | пошлина {r.TollCost}");
        }
        else if (trader.CurrentRoad is Road road)
        {
            Console.WriteLine($"В пути: {road.Name} → {road.Destination?.Name ?? "?"}");
            Console.WriteLine($"   Прогресс: {trader.TurnsOnRoad}/{road.TravelTime}");
            Console.WriteLine($"   Bandit chance: {road.BanditChance:P0} | Friend chance: {road.FriendlyChance:P0}");
        }
    }
}