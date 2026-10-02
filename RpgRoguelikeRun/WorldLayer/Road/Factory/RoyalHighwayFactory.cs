namespace RpgRoguelikeRun.WorldLayer.Roads.Factory;

public class RoyalHighwayFactory : RoadFactory
{
    public RoyalHighwayFactory(int baseLength, double lengthVariance = 0.3)
        : base(baseLength, lengthVariance) { }

    public override Road CreateRoad(Random random)
        => new RoyalHighway(RollLength(random));
}