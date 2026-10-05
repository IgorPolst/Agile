namespace RpgRoguelikeRun.Entities.Events;

public class CloudyDayFactory : RoadEventFactory
{
    public override bool ProducesHostile => true;
    public override RoadEvent CreateEvent() => new CloudyDay();
}