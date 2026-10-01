namespace RpgRoguelikeRun.WorldLayer.Roads;


public class ForestPath : Road
{
    public override string Name => "Лесная тропинка";

    public override int TollCost => 5;

    public override double Safety => 0.55;

    public ForestPath(int length = 6)
    {
        Length = length;
        Quality = RoadQuality.Dirt;
    }

    public override void ApplyStorm()
    {
        Quality = RoadQuality.Muddy;

        Console.WriteLine($"🌧️  {Name}: тропинка размокла, идти тяжело. Качество → {Quality}");
    }
}