using RpgRoguelikeRun.Entities.Events;
using RpgRoguelikeRun.Enums;
using RpgRoguelikeRun.WorldLayer;
using RpgRoguelikeRun.WorldLayer.Locations.Factory;
using RpgRoguelikeRun.WorldLayer.Roads;
using RpgRoguelikeRun.WorldLayer.Roads.Factory;
using RpgRoguelikeRun.Services.Random;
using RpgRoguelikeRun.WorldLayer.Regions;


namespace RpgRoguelikeRun.Configuration;

public static class WorldConfigurator
{

    public static void Configure(World world, Difficulty difficulty)
    {
        ConfigureEventFactories(world, difficulty);
        ConfigureRoadFactories(world, difficulty);
        ConfigureLocations(world);
    }

    // ---------- События ----------

    private static void ConfigureEventFactories(World world, Difficulty difficulty)
    {
        // ---------- Бандиты ----------
        var banditConfig = difficulty switch
        {
            Difficulty.Easy   => (damage: 10, stealChance: 0.2, maxStolen: 1),
            Difficulty.Normal => (damage: 25, stealChance: 0.3, maxStolen: 2),
            Difficulty.Hard   => (damage: 40, stealChance: 0.5, maxStolen: 3),
            _                 => (damage: 25, stealChance: 0.3, maxStolen: 2)
        };

        world.RegisterEventFactory(new BanditAmbushFactory(
            banditConfig.damage,
            banditConfig.stealChance,
            banditConfig.maxStolen));

        // ---------- Шторм ----------
        var stormConfig = difficulty switch
        {
            Difficulty.Easy   => 1,
            Difficulty.Normal => 1,
            Difficulty.Hard   => 2,
            _                 => 1
        };

        world.RegisterEventFactory(new StormFactory(stormConfig));

        // ---------- Купец ----------
        var merchantConfig = difficulty switch
        {
            Difficulty.Easy   => 40,
            Difficulty.Normal => 25,
            Difficulty.Hard   => 15,
            _                 => 25
        };

        world.RegisterEventFactory(new HelpfulMerchantFactory(merchantConfig));

        // ---------- Новые эффекты дороги ----------
        world.RegisterEventFactory(new SunnyDayFactory());
        world.RegisterEventFactory(new CloudyDayFactory());
        world.RegisterEventFactory(new WanderingKnightFactory());
    }

    // ---------- Дороги ----------

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

    // ---------- Локации ----------

    private static void ConfigureLocations(World world)
    {
        var locations = LocationPresets
            .All
            .Select(preset =>
            {
                var region = RegionRegistry.Get(preset.RegionKey);
                return LocationFactoryRegistry.Create(preset.Type, preset.Name, region);
            })
            .ToArray();

        foreach (var loc in locations)
            world.AddLocation(loc);

        ConnectRandomRoads(world, locations);
    }

    private static void ConnectRandomRoads(World world, Location[] locations)
    {
        const int MaxRoadsPerLocation = 4;

        foreach (var location in locations)
        {
            int maxRoads = Math.Min(MaxRoadsPerLocation, locations.Length - 1);
            int roadCount = GameRandom.Next(1, maxRoads + 1);

            var destinations = locations
                .Where(l => l != location)
                .OrderBy(_ => GameRandom.Next(0, int.MaxValue))
                .Take(roadCount);

            foreach (var dest in destinations)
            {
                Road road = world.CreateRandomRoad();
                location.ConnectTo(dest, road);
            }
        }
    }
}