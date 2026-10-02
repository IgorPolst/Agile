using RpgRoguelikeRun.Entities.Events;
using RpgRoguelikeRun.Items;
using RpgRoguelikeRun.WorldLayer;
using RpgRoguelikeRun.WorldLayer.Roads;

namespace RpgRoguelikeRun.Entities;

public class Trader : Creature
{
    public Inventory Inventory { get; }

    public bool IsDelayed { get; private set; }
    public int DaysDelayed { get; private set; }

    // ---------- Текущее местоположение ----------
    public Location? CurrentLocation { get; private set; }
    public Road? CurrentRoad { get; private set; }
    public int TurnsOnRoad { get; private set; }

    // ---------- История событий ----------
    public RoadEvent? LastEvent { get; private set; }
    public List<RoadEvent> EventLog { get; } = new();

    public Trader(string name, int gold = 0, int capacity = 20)
        : base(name, gold)
    {
        Inventory = new Inventory(capacity);
    }

    // ---------- Движение ----------
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

    public override void Move() { }

    // ---------- Локации ----------
    public void EnterLocation(Location location)
    {
        CurrentLocation = location;
        CurrentRoad = null;
        TurnsOnRoad = 0; 
        Console.WriteLine($"🏘️  {Name} прибыл в {location.Name} ({location.Type}).");
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
        Inventory.AgeItems(1);   // 👈 товары портятся в пути

        return TurnsOnRoad >= CurrentRoad.TravelTime;
    }

    // ---------- Дороги ----------
    public Location? PreviousLocation { get; private set; }
    public void EnterRoad(Road road)
    {
        PreviousLocation = CurrentLocation;
        CurrentRoad = road;
        CurrentLocation = null;
        TurnsOnRoad = 0;    
        Console.WriteLine($"🛤️  {Name} вышел на дорогу: {road.Name}");
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

    // ---------- Торговля ----------
    public bool Buy(Item item, int quantity)
    {
        if (CurrentLocation == null)
        {
            Console.WriteLine("❌ Здесь нет рынка.");
            return false;
        }

        var market = CurrentLocation.Market;

        // Находим лот
        var lot = market.Lots.FirstOrDefault(l =>
            l.Item.Name == item.Name &&
            l.Item.Category == item.Category &&
            l.Item.Quality == item.Quality);

        if (lot == null || lot.Quantity < quantity)
        {
            Console.WriteLine("❌ Товара нет на рынке или его не хватает.");
            return false;
        }

        int pricePerUnit = market.GetBuyPrice(lot);
        int totalPrice = pricePerUnit * quantity;

        if (Gold < totalPrice)
        {
            Console.WriteLine($"❌ Не хватает золота: нужно {totalPrice}, есть {Gold}.");
            return false;
        }

        if (!Inventory.HasSpaceFor(quantity))
        {
            Console.WriteLine($"❌ Не хватает места: нужно {quantity}, свободно {Inventory.Capacity - Inventory.Count}.");
            return false;
        }

        LoseGold(totalPrice);
        Inventory.Add(item, quantity);

        lot.Quantity -= quantity;
        lot.RegisterTrade(quantity, isSale: false);   // 👈 цена растёт

        if (lot.Quantity == 0)
            market.Lots.Remove(lot);

        Console.WriteLine($"🛒 Куплено: {item.Name} x{quantity} по {pricePerUnit} = {totalPrice}. Осталось: {Gold}");
        return true;
    }


        public bool Sell(Item item, int quantity)
    {
        if (CurrentLocation == null)
        {
            Console.WriteLine("❌ Здесь нет рынка.");
            return false;
        }

        int available = Inventory.CountOf(item);
        if (available < quantity)
        {
            Console.WriteLine($"❌ Не хватает товара: нужно {quantity}, есть {available}.");
            return false;
        }

        var market = CurrentLocation.Market;
        int pricePerUnit = market.GetSellPrice(item, quantity);
        int totalPrice = pricePerUnit * quantity;

        if (!Inventory.Remove(item, quantity)) return false;
        AddGold(totalPrice);

        // Регистрируем продажу — цена падает
        var lot = market.Lots.FirstOrDefault(l =>
            l.Item.Name == item.Name &&
            l.Item.Category == item.Category &&
            l.Item.Quality == item.Quality);

        if (lot != null)
        {
            lot.RegisterTrade(quantity, isSale: true);
        }
        else
        {
            // Создаём новый лот от продавца (опционально)
            market.AddLot((Item)item.Clone(), quantity, pricePerUnit);
            market.Lots.Last().RegisterTrade(quantity, isSale: true);
        }

        Console.WriteLine($"💰 Продано: {item.Name} x{quantity} по {pricePerUnit} = {totalPrice}. Теперь золота: {Gold}");
        return true;
    }

    public void Trade() { }
}