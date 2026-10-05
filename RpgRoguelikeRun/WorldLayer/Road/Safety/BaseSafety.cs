using RpgRoguelikeRun.WorldLayer.Roads;

namespace RpgRoguelikeRun.WorldLayer.Roads.Safety;

public class BaseSafety : ISafetyModifier
{
    public double GetSafety(Road road) => road.Safety;
}