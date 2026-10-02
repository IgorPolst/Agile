namespace RpgRoguelikeRun.WorldLayer.Roads.Factory;

public abstract class RoadFactory
{
    protected int BaseLength { get; }
    protected double LengthVariance { get; }

    protected RoadFactory(int baseLength, double lengthVariance = 0.3)
    {
        BaseLength = baseLength;
        LengthVariance = lengthVariance;
    }

    protected int RollLength(Random random)
    {
        double min = BaseLength * (1 - LengthVariance);
        double max = BaseLength * (1 + LengthVariance);
        return Math.Max(1, (int)Math.Round(min + random.NextDouble() * (max - min)));
    }

    public abstract Road CreateRoad(Random random);
}