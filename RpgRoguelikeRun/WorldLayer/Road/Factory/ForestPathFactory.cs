using RpgRoguelikeRun.Services.Random;

namespace RpgRoguelikeRun.WorldLayer.Roads.Factory;

public class ForestPathFactory : RoadFactory
{
    public ForestPathFactory(int baseLength, double lengthVariance = 0.5)
        : base(baseLength, lengthVariance) { }

    public override Road CreateRoad(IRandomProvider random)
        => new ForestPath(RollLength(random));
}