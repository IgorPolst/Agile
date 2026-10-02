using RpgRoguelikeRun.Entities;
using RpgRoguelikeRun.WorldLayer.Roads;

namespace RpgRoguelikeRun.UI;

public static class TravelMenu
{
    public static Road? ChooseRoad(Trader trader)
    {
        if (trader.CurrentLocation == null)
        {
            Console.WriteLine("Вы не в локации.");
            Console.ReadKey(true);
            return null;
        }

        var roads = trader.CurrentLocation.OutgoingRoads;
        if (roads.Count == 0)
        {
            Console.WriteLine("Из этой локации нет дорог.");
            Console.ReadKey(true);
            return null;
        }

        Console.Clear();
        Console.WriteLine($"=== ВЫХОД ИЗ {trader.CurrentLocation.Name} ===");
        for (int i = 0; i < roads.Count; i++)
        {
            var r = roads[i];
            Console.WriteLine($"  [{i + 1}] → {r.Destination?.Name ?? "?"} | {r.Name} | {r.TravelTime} ходов | пошлина {r.TollCost}");
        }
        Console.WriteLine("  [0] Остаться");

        Console.Write("Куда идём? ");
        if (!int.TryParse(Console.ReadLine(), out int choice)) return null;
        if (choice <= 0 || choice > roads.Count) return null;

        return roads[choice - 1];
    }
}