namespace RpgRoguelikeRun.WorldLayer.Roads;

public class AbandonedRoad : Road
{
    public override string Name => "Заброшенная дорога";

    public override int TollCost => 0;

    public override double Safety => 0.25;

    public AbandonedRoad(int length = 15)
    {
        Length = length;
        Quality = RoadQuality.Overgrown;
    }

    public override void ApplyStorm()
    {
        Quality = Quality == RoadQuality.Overgrown
            ? RoadQuality.Dirt
            : RoadQuality.Muddy;

        Console.WriteLine($"🌧️  {Name}: дожди размыли колею. Качество → {Quality}");
    }
}