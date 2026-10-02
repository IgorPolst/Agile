using RpgRoguelikeRun.Entities;
using RpgRoguelikeRun.Entities.Events;
using RpgRoguelikeRun.WorldLayer.Roads;

namespace RpgRoguelikeRun.WorldLayer;

public class World
{
    public WorldMap Map { get; }
    public Trader? Trader { get; private set; }
    public List<Enemy> Enemies { get; } = new();

    public List<RoadEventFactory> EventFactories { get; } = new();
    public List<RoadFactory> RoadFactories { get; } = new();
     public List<Location> Locations { get; } = new();
    public List<Road> Roads { get; } = new();

    private readonly Random _random = new();

    public World(int width, int height)
    {
        Map = new WorldMap(width, height);
    }

    // ---------- Trader / Enemy ----------

    public void AddTrader(Trader trader) => Trader = trader;
    public void AddEnemy(Enemy enemy) => Enemies.Add(enemy);
    public void RemoveEnemy(Enemy enemy) => Enemies.Remove(enemy);

    // ---------- Локации ----------
    public void AddLocation(Location location) => Locations.Add(location);

    public Location GetRandomLocation()
    {
        if (Locations.Count == 0)
            throw new InvalidOperationException("В мире нет локаций.");
        return Locations[_random.Next(Locations.Count)];
    }

    public void ArriveAt(Trader trader)
    {
        if (trader.CurrentRoad?.Destination == null)
        {
            Console.WriteLine("⚠️  Дорога не имеет Destination — торговец остаётся на месте.");
            trader.LeaveRoad();
            return;
        }

        Location destination = trader.CurrentRoad.Destination;
        trader.LeaveRoad();
        trader.EnterLocation(destination);
    }

    // ---------- Регистрация фабрик ----------

    public void RegisterEventFactory(RoadEventFactory factory) => EventFactories.Add(factory);
    public void RegisterRoadFactory(RoadFactory factory) => RoadFactories.Add(factory);

    // ---------- Дороги ----------

    public Road CreateRandomRoad()
    {
        if (RoadFactories.Count == 0)
            throw new InvalidOperationException("Нет зарегистрированных фабрик дорог.");

        RoadFactory factory = RoadFactories[_random.Next(RoadFactories.Count)];
        return factory.CreateRoad(_random);
    }

    public void GenerateRoads(int count)
    {
        for (int i = 0; i < count; i++)
            Roads.Add(CreateRandomRoad());
    }

    // ---------- События ----------
    public RoadEvent? TriggerRandomRoadEvent(Trader trader, Road road)
    {
        if (EventFactories.Count == 0) return null;

        RoadEvent? roadEvent = PickEventForRoad(road);
        if (roadEvent == null) return null;

        roadEvent.Trigger(trader);
        trader.RegisterEvent(roadEvent);

        ApplyEventSideEffects(roadEvent, road);

        return roadEvent;
    }

    private RoadEvent? PickEventForRoad(Road road)
    {
        double roll = _random.NextDouble();

        bool wantHostile = roll < road.BanditChance;
        bool wantFriendly = !wantHostile
                            && roll < road.BanditChance + road.FriendlyChance;

        List<RoadEventFactory> candidates = wantHostile
            ? EventFactories.Where(f => f.ProducesHostile).ToList()
            : wantFriendly
                ? EventFactories.Where(f => f.ProducesFriendly).ToList()
                : EventFactories.Where(f => f.ProducesNeutral).ToList();

        if (candidates.Count == 0)
            candidates = EventFactories.Where(f => f.ProducesNeutral).ToList();

        if (candidates.Count == 0) return null;

        RoadEventFactory factory = candidates[_random.Next(candidates.Count)];
        return factory.CreateEvent();
    }

    private void ApplyEventSideEffects(RoadEvent roadEvent, Road road)
    {
        if (roadEvent.TriggersStormDamage)
            road.ApplyStorm();
    }

    // ---------- Экономика ----------

    public bool TryPayToll(Road road, Trader trader)
    {
        if (trader.Gold < road.TollCost)
        {
            Console.WriteLine($"❌ Не хватает золота на пошлину ({road.TollCost}).");
            return false;
        }

        trader.LoseGold(road.TollCost);
        Console.WriteLine($"💰 Оплачена пошлина {road.TollCost} за {road.Name}. Осталось: {trader.Gold}");
        return true;
    }
}