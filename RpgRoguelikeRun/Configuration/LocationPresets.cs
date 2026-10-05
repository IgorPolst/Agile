using RpgRoguelikeRun.WorldLayer;

namespace RpgRoguelikeRun.Configuration;

public static class LocationPresets
{
    public static readonly (string Name, LocationType Type)[] All =
    {
        
        ("Дубровка",       LocationType.Village),
        ("Берёзовка",      LocationType.Village),
        ("Кленово",        LocationType.Village),
        ("Сосновка",       LocationType.Village),
        ("Ольховка",       LocationType.Village),

        ("Столица",        LocationType.Town),
        ("Североград",     LocationType.Town),
        ("Каменск",        LocationType.Town),
        ("Белгород",       LocationType.Town),
        ("Златоуст",       LocationType.Town),

        ("Каир",           LocationType.Port),
        ("Северный порт",  LocationType.Port),
        ("Вольный порт",   LocationType.Port),
        ("Южный мыс",      LocationType.Port),
        ("Три маяка",      LocationType.Port),

        ("Дорн",           LocationType.Mine),
        ("Глубокий рудник", LocationType.Mine),
        ("Железный кряж",  LocationType.Mine),
        ("Соляная яма",    LocationType.Mine),
        ("Медный холм",    LocationType.Mine),

        ("Аббатство",      LocationType.Monastery),
        ("Святой крест",   LocationType.Monastery),
        ("Тихая обитель",  LocationType.Monastery),
        ("Высокий скит",   LocationType.Monastery),
        ("Лесная келья",   LocationType.Monastery),
    };
}