using RpgRoguelikeRun.Entities;
using RpgRoguelikeRun.Entities.Events;
using RpgRoguelikeRun.WorldLayer.Roads;
using RpgRoguelikeRun.WorldLayer.Roads.Factory;
using RpgRoguelikeRun.Services.Random;

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

    public World(int width, int height)
    {
        Map = new WorldMap(width, height);
    }


    public void AddTrader(Trader trader) => Trader = trader;
    public void AddEnemy(Enemy enemy) => Enemies.Add(enemy);
    public void RemoveEnemy(Enemy enemy) => Enemies.Remove(enemy);


    public void AddLocation(Location location) => Locations.Add(location);

    public Location GetRandomLocation()
    {
        if (Locations.Count == 0)
            throw new InvalidOperationException("В мире нет локаций.");
        return Locations[GameRandom.Next(0, Locations.Count)];
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

    public void RegisterEventFactory(RoadEventFactory factory) => EventFactories.Add(factory);
    public void RegisterRoadFactory(RoadFactory factory) => RoadFactories.Add(factory);


    public Road CreateRandomRoad()
    {
        if (RoadFactories.Count == 0)
            throw new InvalidOperationException("Нет зарегистрированных фабрик дорог.");

        RoadFactory factory = RoadFactories[GameRandom.Next(0, RoadFactories.Count)];
        return factory.CreateRoad(GameRandom.Provider);
    }

    public void GenerateRoads(int count)
    {
        for (int i = 0; i < count; i++)
            Roads.Add(CreateRandomRoad());
    }

    public RoadEvent? TriggerRandomRoadEvent(Trader trader, Road road)
    {
        if (EventFactories.Count == 0) return null;

        RoadEvent? roadEvent = PickEventForRoad(road, trader);
        if (roadEvent == null) return null;

        roadEvent.Trigger(trader);
        trader.RegisterEvent(roadEvent);

        ApplyEventSideEffects(roadEvent, road);

        return roadEvent;
    }

    private RoadEvent? PickEventForRoad(Road road, Trader trader)
    {
        double roll = GameRandom.NextDouble();

        const double NeutralShare = 0.40;
        
        double effectiveBanditChance = Math.Clamp(road.BanditChance + trader.BanditChanceBonus, 0.0, 1.0);

    double hostileThreshold  = effectiveBanditChance * (1 - NeutralShare);
    double friendlyThreshold = hostileThreshold + road.FriendlyChance * (1 - NeutralShare);

        bool wantHostile  = roll < hostileThreshold;
        bool wantFriendly = !wantHostile && roll < friendlyThreshold;

        List<RoadEventFactory> candidates = wantHostile
            ? EventFactories.Where(f => f.ProducesHostile).ToList()
            : wantFriendly
                ? EventFactories.Where(f => f.ProducesFriendly).ToList()
                : EventFactories.Where(f => f.ProducesNeutral).ToList();

        if (candidates.Count == 0)
            candidates = EventFactories.Where(f => f.ProducesNeutral).ToList();

        if (candidates.Count == 0) return null;

        RoadEventFactory factory = candidates[GameRandom.Next(0, candidates.Count)];
        return factory.CreateEvent();
    }

    private void ApplyEventSideEffects(RoadEvent roadEvent, Road road)
    {
        if (roadEvent.TriggersStormDamage)
            road.ApplyStorm();
    }

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