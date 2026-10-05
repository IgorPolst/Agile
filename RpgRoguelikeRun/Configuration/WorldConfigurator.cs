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