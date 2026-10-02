using RpgRoguelikeRun.WorldLayer;

namespace RpgRoguelikeRun.Configuration;

public static class LocationPresets
{
    public static readonly (string Name, LocationType Type)[] All =
    {
        ("Каир",          LocationType.Port),
        ("Дорн",          LocationType.Mine),
        ("Аббатство",     LocationType.Monastery),
        ("Дубровка",      LocationType.Village),
        ("Северный порт", LocationType.Port),
        ("Столица",       LocationType.Town),
    };
}