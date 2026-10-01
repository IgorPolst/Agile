namespace RpgRoguelikeRun.Entities.Events;

public class HelpfulMerchantFactory : RoadEventFactory
{
    private readonly int _bonus;
    public override bool ProducesFriendly => true;

    public HelpfulMerchantFactory(int bonus = 25)
    {
        _bonus = bonus;
    }

    public override RoadEvent CreateEvent() => new HelpfulMerchant { Bonus = _bonus };
}