using RpgRoguelikeRun.Entities.Events;
using RpgRoguelikeRun.Items;
using RpgRoguelikeRun.WorldLayer.Roads;

namespace RpgRoguelikeRun.Entities;

public class Trader : Creature
{
    public Inventory Inventory { get; }

    // ---------- Состояние хода ----------
    public bool IsDelayed { get; private set; }
    public int DaysDelayed { get; private set; }

    // ---------- Текущая дорога ----------
    public Road? CurrentRoad { get; private set; }

    // ---------- История событий ----------
    public RoadEvent? LastEvent { get; private set; }
    public List<RoadEvent> EventLog { get; } = new();

    public Trader(string name, int gold = 0, int capacity = 20)
        : base(name, gold)
    {
        Inventory = new Inventory(capacity);
    }

    // ---------- Движение ----------

    /// <summary>
    /// Попытка сделать ход.
    /// true — торговец сдвинулся (можно триггерить события),
    /// false — пропустил ход из-за задержки.
    /// </summary>
    public bool TryMove()
    {
        if (IsDelayed)
        {
            DaysDelayed--;
            if (DaysDelayed <= 0)
                IsDelayed = false;

            return false;
        }

        Move();
        return true;
    }

    public override void Move()
    {
        // Здесь позже: сдвинуть координаты торговца по карте
    }

    // ---------- Дороги ----------

    public void EnterRoad(Road road)
    {
        CurrentRoad = road;
        Console.WriteLine($"🛤️  {Name} вышел на дорогу: {road.Name}");
    }

    public void LeaveRoad()
    {
        CurrentRoad = null;
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

    // ---------- Торговля (заготовка) ----------

    public void Trade()
    {

    }
}