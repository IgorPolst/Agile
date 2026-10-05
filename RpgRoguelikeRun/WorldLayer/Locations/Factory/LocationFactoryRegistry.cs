using RpgRoguelikeRun.Enums;
using RpgRoguelikeRun.WorldLayer.Regions;

namespace RpgRoguelikeRun.WorldLayer.Locations.Factory;

public static class LocationFactoryRegistry
{
    private static readonly Dictionary<LocationType, LocationFactory> _factories = new()
    {
        { LocationType.Village,   new VillageFactory() },
        { LocationType.Port,      new PortFactory() },
        { LocationType.Mine,      new MineFactory() },
        { LocationType.Monastery, new MonasteryFactory() },
        { LocationType.Town,      new TownFactory() },
    };

    public static LocationFactory Get(LocationType type)
    {
        if (!_factories.TryGetValue(type, out var factory))
            throw new ArgumentException($"Нет фабрики для типа {type}");
        return factory;
    }

    public static Location Create(LocationType type, string name, IRegionStrategy region)
        => Get(type).Create(name, region);
}