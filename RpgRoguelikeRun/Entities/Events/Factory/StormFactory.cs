namespace RpgRoguelikeRun.Entities.Events;

public class StormFactory : RoadEventFactory
{
    private readonly int _delayDays;
    public override bool ProducesNeutral => true;

    public StormFactory(int delayDays = 1)
    {
        _delayDays = delayDays;
    }

    public override RoadEvent CreateEvent() => new Storm { DelayDays = _delayDays };
}