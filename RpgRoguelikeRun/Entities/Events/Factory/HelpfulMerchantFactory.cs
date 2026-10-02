namespace RpgRoguelikeRun.Entities.Events;

public class HelpfulMerchantFactory : RoadEventFactory
{
    private readonly int _bonus;
    private readonly double _giftChance;

    public override bool ProducesFriendly => true;

    public HelpfulMerchantFactory(int bonus = 25, double giftChance = 0.5)
    {
        _bonus = bonus;
        _giftChance = giftChance;
    }

    public override RoadEvent CreateEvent()
        => new HelpfulMerchant { Bonus = _bonus, GiftChance = _giftChance };
}