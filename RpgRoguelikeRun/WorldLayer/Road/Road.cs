using RpgRoguelikeRun.WorldLayer;
using RpgRoguelikeRun.WorldLayer.Roads.Safety;

namespace RpgRoguelikeRun.WorldLayer.Roads;

public abstract class Road
{
    public abstract string Name { get; }
    public int Length { get; protected set; }
    public RoadQuality Quality { get; protected set; }
    public abstract int TollCost { get; }
    public abstract double Safety { get; }
    public Location? Destination { get; set; }

    public double EffectiveSafety => SafetyPipeline.CalculateEffectiveSafety(this);

    public double BanditChance => 1.0 - EffectiveSafety;
    public double FriendlyChance => EffectiveSafety;

    public double SpeedMultiplier => Quality switch
    {
        RoadQuality.Paved    => 1.0,
        RoadQuality.Dirt     => 0.8,
        RoadQuality.Muddy    => 0.5,
        RoadQuality.Overgrown=> 0.6,
        _                    => 1.0
    };
    public int TravelTime => Math.Max(1, (int)Math.Ceiling(Length / SpeedMultiplier));
    public abstract void ApplyStorm();
    public virtual void Repair() => Quality = RoadQuality.Paved;
    public override string ToString()
            => $"{Name} → {Destination?.Name ?? "?"} | length={Length} | quality={Quality} | toll={TollCost}";
}