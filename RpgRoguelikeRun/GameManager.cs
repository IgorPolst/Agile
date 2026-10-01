using RpgRoguelikeRun.Entities;
using RpgRoguelikeRun.Entities.Events;
using RpgRoguelikeRun.Enums;
using RpgRoguelikeRun.WorldLayer;
using RpgRoguelikeRun.WorldLayer.Roads;

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

    // ---------- Init ----------
    private void Init()
    {
        Console.CursorVisible = false;
        _gameStopped = false;

        _world = new World(MapWidth, MapHeight);
        ConfigureEventFactories();
        ConfigureRoadFactories();

        _trader = new Trader("Ганс", gold: 100);
        _world.AddTrader(_trader);
    }

    private void ConfigureEventFactories()
    {
        (int banditDamage, int stormDays, int merchantBonus) = Difficulty switch
        {
            Difficulty.Easy   => (20, 1, 40),
            Difficulty.Normal => (50, 1, 25),
            Difficulty.Hard   => (80, 2, 15),
            _                 => (50, 1, 25)
        };

        _world.RegisterEventFactory(new BanditAmbushFactory(banditDamage));
        _world.RegisterEventFactory(new StormFactory(stormDays));
        _world.RegisterEventFactory(new HelpfulMerchantFactory(merchantBonus));
    }

    private void ConfigureRoadFactories()
    {
        (int royalLen, int abandonedLen, int forestLen) = Difficulty switch
        {
            Difficulty.Easy   => (8, 10, 4),
            Difficulty.Normal => (10, 15, 6),
            Difficulty.Hard   => (12, 20, 8),
            _                 => (10, 15, 6)
        };

        _world.RegisterRoadFactory(new RoyalHighwayFactory(royalLen));
        _world.RegisterRoadFactory(new AbandonedRoadFactory(abandonedLen));
        _world.RegisterRoadFactory(new ForestPathFactory(forestLen));

        _world.GenerateRoads(5);
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
        }
    }

    // ---------- Ход ----------
    private void DoTurn()
    {
        bool moved = _trader.TryMove();
        if (!moved) return;

        if (_trader.CurrentRoad == null)
        {
            Road road = _world.Roads[_random.Next(_world.Roads.Count)];
            _trader.EnterRoad(road);
            _world.TryPayToll(road, _trader);
        }

        if (_random.NextDouble() < 0.3)
        {
            Console.Clear();
            _world.TriggerRandomRoadEvent(_trader, _trader.CurrentRoad!);
            Console.WriteLine();
            Console.WriteLine("Нажмите любую клавишу...");
            Console.ReadKey(true);
        }
    }

    private void Update() { }

    // ---------- Render ----------
    private void Render()
    {
        Console.Clear();
        Console.WriteLine($"=== Medieval Trader | {Difficulty} | {MapWidth}x{MapHeight} ===");
        Console.WriteLine($"Trader: {_trader.Name} | Gold: {_trader.Gold}");

        if (_trader.CurrentRoad is Road currentRoad)
        {
            Console.WriteLine($"Road: {currentRoad.Name} | quality={currentRoad.Quality}");
            Console.WriteLine($"  ⚔️ Bandit chance: {currentRoad.BanditChance:P0} | 🤝 Friend chance: {currentRoad.FriendlyChance:P0}");
        }

        if (_trader.IsDelayed)
            Console.WriteLine($"⏳ Задержан на {_trader.DaysDelayed} ход(ов)");

        if (_trader.LastEvent != null)
            Console.WriteLine($"Last event: {_trader.LastEvent.Title}");

        Console.WriteLine();
        Console.WriteLine("--- Дороги мира ---");
        foreach (var road in _world.Roads)
            Console.WriteLine($"  • {road}");

        Console.WriteLine();
        Console.WriteLine("WASD / Arrows — move, Escape — exit");
    }
}