using RpgRoguelikeRun.Enums;

namespace RpgRoguelikeRun.Configuration;

/// <summary>
/// Пресеты локаций: имя, тип и регион.
/// </summary>
public static class LocationPresets
{
    public static readonly (string Name, LocationType Type, string RegionKey)[] All =
    {
        ("Дубровка",       LocationType.Village, "coastal"),
        ("Берёзовка",      LocationType.Village, "forest"),
        ("Кленово",        LocationType.Village, "mountain"),
        ("Сосновка",       LocationType.Village, "forest"),
        ("Ольховка",       LocationType.Village, "coastal"),

        ("Столица",        LocationType.Town, "forest"),
        ("Североград",     LocationType.Town, "mountain"),
        ("Каменск",        LocationType.Town, "mountain"),
        ("Белгород",       LocationType.Town, "forest"),
        ("Златоуст",       LocationType.Town, "coastal"),

        ("Каир",           LocationType.Port, "forest"),
        ("Северный порт",  LocationType.Port, "mountain"),
        ("Вольный порт",   LocationType.Port, "coastal"),
        ("Южный мыс",      LocationType.Port, "coastal"),
        ("Три маяка",      LocationType.Port, "coastal"),

        ("Дорн",           LocationType.Mine, "mountain"),
        ("Глубокий рудник", LocationType.Mine, "mountain"),
        ("Железный кряж",  LocationType.Mine, "forest"),
        ("Соляная яма",    LocationType.Mine, "coastal"),
        ("Медный холм",    LocationType.Mine, "mountain"),

        ("Аббатство",      LocationType.Monastery, "forest"),
        ("Святой крест",   LocationType.Monastery, "mountain"),
        ("Тихая обитель",  LocationType.Monastery, "coastal"),
        ("Высокий скит",   LocationType.Monastery, "mountain"),
        ("Лесная келья",   LocationType.Monastery, "forest"),
    };
}