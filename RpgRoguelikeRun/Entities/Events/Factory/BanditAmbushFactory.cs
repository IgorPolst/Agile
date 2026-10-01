namespace RpgRoguelikeRun.Entities.Events;

public class BanditAmbushFactory : RoadEventFactory
{
    private readonly int _damage;
    public override bool ProducesHostile => true;

    public BanditAmbushFactory(int damage = 50)
    {
        _damage = damage;
    }

    public override RoadEvent CreateEvent() => new BanditAmbush { Damage = _damage };
}