namespace RpgRoguelikeRun.WorldLayer.Roads;

public class ForestPathFactory : RoadFactory
{
    public ForestPathFactory(int baseLength, double lengthVariance = 0.5)
        : base(baseLength, lengthVariance) { }

    public override Road CreateRoad(Random random)
        => new ForestPath(RollLength(random));
}