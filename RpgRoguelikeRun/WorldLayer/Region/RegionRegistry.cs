namespace RpgRoguelikeRun.WorldLayer.Regions;

public static class RegionRegistry
{
    private static readonly Dictionary<string, IRegionStrategy> _regions = new()
    {
        { "coastal",  new CoastalRegionStrategy() },
        { "forest",   new ForestRegionStrategy() },
        { "mountain", new MountainRegionStrategy() },
    };

    public static IRegionStrategy Get(string key)
    {
        if (!_regions.TryGetValue(key, out var region))
            throw new ArgumentException($"Неизвестный регион: {key}");
        return region;
    }

    public static IEnumerable<string> Keys => _regions.Keys;
}