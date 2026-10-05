using RpgRoguelikeRun.Entities;
using RpgRoguelikeRun.UI;
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
            var dest = r.Destination;
            string destLabel = dest != null
                ? $"{dest.Type}, {dest.Name}, регион: {dest.Region.Name}"
                : "?";
            Console.WriteLine($"  [{i + 1}] → {destLabel} | {r.Name} | {r.TravelTime} ходов | пошлина {r.TollCost}");
        }
        Console.WriteLine("  [0] Остаться");

        Console.WriteLine("Куда идём? (Escape — отмена)");
        int choice = ConsoleReader.ReadChoice(roads.Count);
        if (choice == 0) return null;

        return roads[choice - 1];
    }
}