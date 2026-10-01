namespace RpgRoguelikeRun.WorldLayer.Roads;

public class RoyalHighway : Road
{
    public override string Name => "Королевский тракт";

    public override int TollCost => 25;

    public override double Safety => 0.85;

    public RoyalHighway(int length = 10)
    {
        Length = length;
        Quality = RoadQuality.Paved;
    }

    public override void ApplyStorm()
    {
        Quality = Quality == RoadQuality.Paved
            ? RoadQuality.Dirt
            : RoadQuality.Muddy;

        Console.WriteLine($"🌧️  {Name}: шторм слегка размыл тракт. Качество → {Quality}");
    }

    public override void Repair()
    {
        Quality = RoadQuality.Paved;
        Console.WriteLine($"🔨 {Name}: королевские ремонтники восстановили тракт.");
    }
}