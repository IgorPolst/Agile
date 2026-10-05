namespace RpgRoguelikeRun.Entities.Events;

public class WanderingKnightFactory : RoadEventFactory
{
    public override bool ProducesFriendly => true;
    public override RoadEvent CreateEvent() => new WanderingKnight();
}