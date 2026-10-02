using RpgRoguelikeRun.Entities.Events;
using RpgRoguelikeRun.Enums;
using RpgRoguelikeRun.WorldLayer;
using RpgRoguelikeRun.WorldLayer.Roads;
using RpgRoguelikeRun.WorldLayer.Locations;

namespace RpgRoguelikeRun.Configuration;

public static class WorldConfigurator
{
    private static readonly Random _random = new();
    public static void Configure(World world, Difficulty difficulty)
    {
        ConfigureEventFactories(world, difficulty);
        ConfigureRoadFactories(world, difficulty);
        ConfigureLocations(world);
    }

    private static void ConfigureEventFactories(World world, Difficulty difficulty)
    {
        var config = difficulty switch
        {
            Difficulty.Easy   => (banditDamage: 20, stormDays: 1, merchantBonus: 40),
            Difficulty.Normal => (banditDamage: 50, stormDays: 1, merchantBonus: 25),
            Difficulty.Hard   => (banditDamage: 80, stormDays: 2, merchantBonus: 15),
            _                 => (banditDamage: 50, stormDays: 1, merchantBonus: 25)
        };

        world.RegisterEventFactory(new BanditAmbushFactory(config.banditDamage));
        world.RegisterEventFactory(new StormFactory(config.stormDays));
        world.RegisterEventFactory(new HelpfulMerchantFactory(config.merchantBonus));
    }

    private static void ConfigureRoadFactories(World world, Difficulty difficulty)
    {
        var config = difficulty switch
        {
            Difficulty.Easy   => (royalLen: 8,  abandonedLen: 10, forestLen: 4),
            Difficulty.Normal => (royalLen: 10, abandonedLen: 15, forestLen: 6),
            Difficulty.Hard   => (royalLen: 12, abandonedLen: 20, forestLen: 8),
            _                 => (royalLen: 10, abandonedLen: 15, forestLen: 6)
        };

        world.RegisterRoadFactory(new RoyalHighwayFactory(config.royalLen));
        world.RegisterRoadFactory(new AbandonedRoadFactory(config.abandonedLen));
        world.RegisterRoadFactory(new ForestPathFactory(config.forestLen));
    }

    private static void ConfigureLocations(World world)
{
    // 1. Создаём локации через реестр фабрик
    var cairo   = LocationFactoryRegistry.Create(LocationType.Port,      "Каир");
    var dorn    = LocationFactoryRegistry.Create(LocationType.Mine,      "Дорн");
    var abbey   = LocationFactoryRegistry.Create(LocationType.Monastery, "Аббатство");
    var village = LocationFactoryRegistry.Create(LocationType.Village,   "Дубровка");
    var port    = LocationFactoryRegistry.Create(LocationType.Port,      "Северный порт");
    var town    = LocationFactoryRegistry.Create(LocationType.Town,      "Столица");

    var all = new[] { cairo, dorn, abbey, village, port, town };
    foreach (var loc in all)
        world.AddLocation(loc);

    foreach (var location in all)
    {
        int maxRoads = Math.Min(4, all.Length - 1);
        int roadCount = _random.Next(1, maxRoads + 1);

        var destinations = all
            .Where(l => l != location)
            .OrderBy(_ => _random.Next())
            .Take(roadCount)
            .ToArray();

        foreach (var dest in destinations)
        {
            Road road = world.CreateRandomRoad();
            location.ConnectTo(dest, road);
        }
    }
}


    private static void Connect(World world, Location from, Location[] destinations)
    {
        foreach (var to in destinations)
        {
            Road road = world.CreateRandomRoad();
            from.ConnectTo(to, road);
        }
    }

    
}