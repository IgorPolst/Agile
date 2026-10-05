using RpgRoguelikeRun.WorldLayer.Roads;

namespace RpgRoguelikeRun.WorldLayer.Roads.Safety;

public interface ISafetyModifier
{
    double GetSafety(Road road);
}