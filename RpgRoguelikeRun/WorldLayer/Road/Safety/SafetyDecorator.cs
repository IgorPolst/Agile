using RpgRoguelikeRun.WorldLayer.Roads;

namespace RpgRoguelikeRun.WorldLayer.Roads.Safety;

public abstract class SafetyDecorator : ISafetyModifier
{
    protected readonly ISafetyModifier _inner;

    protected SafetyDecorator(ISafetyModifier inner)
    {
        _inner = inner ?? throw new ArgumentNullException(nameof(inner));
    }

    public virtual double GetSafety(Road road) => _inner.GetSafety(road);
}