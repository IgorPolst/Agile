namespace RpgRoguelikeRun.Entities.Events;

public class BanditAmbushFactory : RoadEventFactory
{
    private readonly int _damage;
    private readonly double _stealChance;
    private readonly int _maxStolenItems;
    public override bool ProducesHostile => true;

    public BanditAmbushFactory(int damage = 50, double stealChance = 0.5, int maxStolenItems = 2)
    {
        _damage = damage;
        _stealChance = stealChance;
        _maxStolenItems = maxStolenItems;
    }

    public override RoadEvent CreateEvent() => new BanditAmbush
    {
        Damage = _damage,
        StealChance = _stealChance,
        MaxStolenItems = _maxStolenItems
    };
}