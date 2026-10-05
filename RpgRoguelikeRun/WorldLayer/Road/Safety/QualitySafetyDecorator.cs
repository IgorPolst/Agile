using RpgRoguelikeRun.WorldLayer.Roads;

namespace RpgRoguelikeRun.WorldLayer.Roads.Safety;

public class QualitySafetyDecorator : SafetyDecorator
{
    public QualitySafetyDecorator(ISafetyModifier inner) : base(inner) { }

    public override double GetSafety(Road road)
    {
        double baseSafety = _inner.GetSafety(road);

        double multiplier = road.Quality switch
        {
            RoadQuality.Paved     => 1.00,
            RoadQuality.Dirt      => 0.90,
            RoadQuality.Muddy     => 0.65,
            RoadQuality.Overgrown => 0.50,
            _                     => 1.00
        };

        return baseSafety * multiplier;
    }
}