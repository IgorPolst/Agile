using RpgRoguelikeRun.Entities.Events;
using RpgRoguelikeRun.Items;
using RpgRoguelikeRun.Services;
using RpgRoguelikeRun.WorldLayer;
using RpgRoguelikeRun.WorldLayer.Roads;

namespace RpgRoguelikeRun.Entities;

public class Trader : Creature
{
    public Inventory Inventory { get; }

    // ---------- Состояние хода ----------
    public bool IsDelayed { get; private set; }
    public int DaysDelayed { get; private set; }

    // ---------- Текущее местоположение ----------
    public Location? CurrentLocation { get; private set; }
    public Location? PreviousLocation { get; private set; }
    public Road? CurrentRoad { get; private set; }
    public int TurnsOnRoad { get; private set; }

    // ---------- История событий ----------
    public RoadEvent? LastEvent { get; private set; }
    public List<RoadEvent> EventLog { get; } = new();

    public Trader(string name, int gold = 0, int capacity = 20)
        : base(name, gold)
    {
        Inventory = new Inventory(capacity);
        Inventory.OnItemAdded += SubscribeToItem;
        Inventory.OnItemRemoved += UnsubscribeFromItem;
    }

    // ---------- История сделок ----------
    public List<TradeRecord> TradeHistory { get; } = new();

    private const int MaxHistorySize = 10;

    public void RegisterTrade(TradeRecord record)
    {
        TradeHistory.Insert(0, record);
        if (TradeHistory.Count > MaxHistorySize)
            TradeHistory.RemoveAt(TradeHistory.Count - 1);
    }

    // ---------- Ход ----------
    public bool TryMove()
    {
        if (IsDelayed)
        {
            DaysDelayed--;
            if (DaysDelayed <= 0) IsDelayed = false;
            return false;
        }

        Move();
        Inventory.AgeItems(1);
        return true;
    }

    public override void Move() { /* сюда позже: движение по карте */ }

    // ---------- Локации ----------
    public void EnterLocation(Location location)
    {
        CurrentLocation = location;
        CurrentRoad = null;
        TurnsOnRoad = 0;
    }

    // ---------- Дороги ----------
    public void EnterRoad(Road road)
    {
        PreviousLocation = CurrentLocation;
        CurrentLocation = null;
        CurrentRoad = road;
        TurnsOnRoad = 0;
    }

    public void LeaveRoad()
    {
        CurrentRoad = null;
        TurnsOnRoad = 0;
    }

    public bool StepOnRoad()
    {
        if (CurrentRoad == null) return false;

        TurnsOnRoad++;
        Inventory.AgeItems(1);
        return TurnsOnRoad >= CurrentRoad.TravelTime;
    }

    // ---------- Экономика ----------
    public void AddGold(int amount)
    {
        if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
        Gold += amount;
    }

    public int LoseGold(int amount)
    {
        if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
        int actual = Math.Min(Gold, amount);
        Gold -= actual;
        return actual;
    }

    // ---------- Эффекты событий ----------
    public void Delay(int days)
    {
        IsDelayed = true;
        DaysDelayed += days;
    }

    public void RegisterEvent(RoadEvent roadEvent)
    {
        LastEvent = roadEvent;
        EventLog.Add(roadEvent);
    }

    // ---------- Торговля (делегаты в TradeService) ----------
    public string? Buy(Item item, int quantity) => TradeService.Buy(this, item, quantity);
    public string? Sell(Item item, int quantity) => TradeService.Sell(this, item, quantity);

    // ---------- Observer: подписка на порчу товаров ----------

    public List<Item> SpoiledLog { get; } = new();

    public string? LastSpoiledMessage { get; private set; }

    public void SubscribeToItem(Item item)
    {
        item.OnSpoiled += HandleItemSpoiled;
    }

    public void UnsubscribeFromItem(Item item)
    {
        item.OnSpoiled -= HandleItemSpoiled;
    }

    private void HandleItemSpoiled(Item item)
    {
        string message = $"⚠️ {item.Name} испортился! Цена упала до {item.CurrentCost}.";
        SpoiledLog.Add(item);
        LastSpoiledMessage = message;
        Console.WriteLine(message);
    }
}