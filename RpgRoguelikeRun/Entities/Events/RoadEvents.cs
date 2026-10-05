namespace RpgRoguelikeRun.Entities.Events;
public abstract class RoadEvent
{
    public abstract string Title { get; }
    public virtual bool TriggersStormDamage => false;
    public virtual bool IsHostile => false;
    public virtual bool IsFriendly => false;
    public abstract void Trigger(Trader trader);
}