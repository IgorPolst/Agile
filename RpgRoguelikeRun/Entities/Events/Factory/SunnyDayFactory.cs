namespace RpgRoguelikeRun.Entities.Events;

public class SunnyDayFactory : RoadEventFactory
{
    public override bool ProducesFriendly => true;
    public override RoadEvent CreateEvent() => new SunnyDay();
}