using RpgRoguelikeRun.Services.Random;
namespace RpgRoguelikeRun.WorldLayer.Roads.Factory;

public class AbandonedRoadFactory : RoadFactory
{
    public AbandonedRoadFactory(int baseLength, double lengthVariance = 0.4)
        : base(baseLength, lengthVariance) { }

    public override Road CreateRoad(IRandomProvider random)
        => new AbandonedRoad(RollLength(random));
}