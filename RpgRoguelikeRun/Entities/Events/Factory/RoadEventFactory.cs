namespace RpgRoguelikeRun.Entities.Events;

public abstract class RoadEventFactory
{
    public abstract RoadEvent CreateEvent();
    public virtual bool ProducesHostile => false;
    public virtual bool ProducesFriendly => false;
    public virtual bool ProducesNeutral => false;
}