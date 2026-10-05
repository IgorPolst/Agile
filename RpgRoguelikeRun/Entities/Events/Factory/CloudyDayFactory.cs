namespace RpgRoguelikeRun.Entities.Events;

public class CloudyDayFactory : RoadEventFactory
{
    public override bool ProducesFriendly => true;
    public override RoadEvent CreateEvent() => new SunnyDay();
}