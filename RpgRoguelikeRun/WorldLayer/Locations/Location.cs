using RpgRoguelikeRun.WorldLayer.Roads;

namespace RpgRoguelikeRun.WorldLayer;

public class Location
{
    public string Name { get; }
    public LocationType Type { get; }
    public Market Market { get; }

    public List<Road> OutgoingRoads { get; } = new();

    public Location(string name, LocationType type, Market market)
    {
        Name = name;
        Type = type;
        Market = market;
    }

    public void AddRoad(Road road)
    {
        if (road.Destination == null)
            throw new InvalidOperationException($"Дорога {road.Name} не имеет Destination.");

        OutgoingRoads.Add(road);
    }

    public Road ConnectTo(Location other, Road road)
    {
        road.Destination = other;
        OutgoingRoads.Add(road);
        return road;
    }

    public override string ToString() => $"{Name} ({Type})";
}