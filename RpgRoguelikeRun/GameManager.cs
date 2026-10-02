using RpgRoguelikeRun.Entities;
using RpgRoguelikeRun.Enums;
using RpgRoguelikeRun.WorldLayer;
using RpgRoguelikeRun.WorldLayer.Roads;
using RpgRoguelikeRun.UI;
using RpgRoguelikeRun.Configuration;


namespace RpgRoguelikeRun;

public sealed class GameManager
{
    // ---------- Singleton ----------
    private static GameManager? _instance;
    public static GameManager Instance => _instance ??= new GameManager();

    private GameManager()
    {
        MapWidth = 80;
        MapHeight = 25;
        Difficulty = Difficulty.Normal;
        _random = new Random();
    }

    // ---------- Настройки ----------
    public int MapWidth { get; set; }
    public int MapHeight { get; set; }
    public Difficulty Difficulty { get; set; }

    // ---------- Состояние ----------
    private World _world = null!;
    private Trader _trader = null!;
    private bool _gameStopped;
    private readonly Random _random;
    private int _turnsOnRoad = 0;

    // ---------- Init ----------
    private void Init()
    {
        Console.CursorVisible = false;
        _gameStopped = false;

        _world = new World(MapWidth, MapHeight);
        WorldConfigurator.Configure(_world, Difficulty);

        _trader = new Trader("Ганс", gold: 100);
        _world.AddTrader(_trader);
        _trader = TraderFactory.CreateStartingTrader("Ганс", gold: 100);

        var startLocation = _world.Locations.First();
        _trader.EnterLocation(startLocation);

    }

    private void TravelFromLocation()
    {
        if (_trader.CurrentLocation == null)
        {
            Console.WriteLine("Вы уже в пути. Нажмите WASD, чтобы идти.");
            Console.ReadKey(true);
            return;
        }

        var road = TravelMenu.ChooseRoad(_trader);
        if (road == null) return;

        if (!_world.TryPayToll(road, _trader)) return;

        _trader.EnterRoad(road);
        Console.WriteLine($"Вы отправились в путь по {road.Name} → {road.Destination?.Name ?? "?"}.");
        Console.WriteLine($"Длина дороги: {road.TravelTime} ходов. Жмите WASD, чтобы идти.");
        Console.ReadKey(true);
    }
    // ---------- Run ----------
    public void Run()
    {
        Init();

        Console.Clear();
        Console.WriteLine($"Game Started with difficulty: {Difficulty}");
        Console.WriteLine($"Map size: {MapWidth}x{MapHeight}");
        Console.WriteLine("Press any key to start...");
        Console.ReadKey(true);

        while (!_gameStopped)
        {
            HandleInput();
            Update();
            Render();
        }

        Console.CursorVisible = true;
        Console.WriteLine($"Game over. Final gold: {_trader.Gold}");
        Console.WriteLine("Press any key to exit.");
        Console.ReadKey(true);
    }

    // ---------- Input ----------
    private void HandleInput()
    {
        var key = Console.ReadKey(true);

        switch (key.Key)
        {
            case ConsoleKey.Escape:
                _gameStopped = true;
                break;

            case ConsoleKey.UpArrow:
            case ConsoleKey.W:
            case ConsoleKey.DownArrow:
            case ConsoleKey.S:
            case ConsoleKey.LeftArrow:
            case ConsoleKey.A:
            case ConsoleKey.RightArrow:
            case ConsoleKey.D:
                DoTurn();
                break;
            case ConsoleKey.B:
                MarketMenu.Open(_trader);
                break;
            case ConsoleKey.T:
                TravelFromLocation();
                break;
        }
    }

    // ---------- Ход ----------
        private void DoTurn()
    {
        if (_trader.CurrentRoad != null)
        {
            bool arrived = _trader.StepOnRoad();
            TryTriggerRoadEvent();

            if (arrived)
            {
                _world.ArriveAt(_trader);
                Console.ReadKey(true);
            }
        }
        else
        {
            _trader.TryMove();
        }

        CheckBankruptcy();      
    }

    private void CheckBankruptcy()
    {
        if (_trader.Gold <= 0 && _trader.Inventory.Stacks.Count == 0)
        {
            _gameStopped = true;
            Console.Clear();
            Console.WriteLine("💀 Вы обанкротились! Игра окончена.");
            Console.WriteLine($"Событий пережито: {_trader.EventLog.Count}");
            Console.ReadKey(true);
        }
    }

        private void TryTriggerRoadEvent()
    {
        if (_trader.CurrentRoad == null) return;

        double chance = Difficulty switch
        {
            Difficulty.Easy   => 0.20,
            Difficulty.Normal => 0.30,
            Difficulty.Hard   => 0.45,
            _                 => 0.30
        };

        if (_random.NextDouble() < chance)
        {
            Console.Clear();
            _world.TriggerRandomRoadEvent(_trader, _trader.CurrentRoad);
            Console.WriteLine();
            Console.WriteLine("Нажмите любую клавишу...");
            Console.ReadKey(true);
        }
    }


    private void Update()
    {
    }

    // ---------- Render ----------
   private void Render()
    {
        Console.Clear();
        Console.WriteLine($"=== Medieval Trader | {Difficulty} | {MapWidth}x{MapHeight} ===");
        Console.WriteLine($"Trader: {_trader.Name} | Gold: {_trader.Gold}");

        // Инвентарь
        Console.WriteLine();
        Console.WriteLine("--- Инвентарь ---");
        if (_trader.Inventory.Stacks.Count == 0)
            Console.WriteLine("  (пусто)");
        else
            foreach (var stack in _trader.Inventory.Stacks)
                Console.WriteLine($"  • {stack}");

        // Текущее местоположение
        Console.WriteLine();
        if (_trader.CurrentLocation is Location loc)
        {
            Console.WriteLine($"📍 Локация: {loc.Name} ({loc.Type})");
            Console.WriteLine($"   Рынок: {loc.Market.Lots.Count} лотов");
            Console.WriteLine($"   Дорог отсюда: {loc.OutgoingRoads.Count}");
            foreach (var r in loc.OutgoingRoads)
                Console.WriteLine($"      → {r.Destination?.Name ?? "?"} | {r.TravelTime} ходов | пошлина {r.TollCost}");
        }
        else if (_trader.CurrentRoad is Road road)
        {
            Console.WriteLine($"В пути: {road.Name} → {road.Destination?.Name ?? "?"}");
            Console.WriteLine($"   Прогресс: {_trader.TurnsOnRoad}/{road.TravelTime}");
            Console.WriteLine($"   Bandit chance: {road.BanditChance:P0} | Friend chance: {road.FriendlyChance:P0}");
        }

        if (_trader.IsDelayed)
            Console.WriteLine($"Задержан на {_trader.DaysDelayed} ход(ов)");

        if (_trader.LastEvent != null)
            Console.WriteLine($"Last event: {_trader.LastEvent.Title}");

        Console.WriteLine();
        Console.WriteLine("WASD — идти, B — рынок, T — выйти из локации, Escape — exit");
    }
}