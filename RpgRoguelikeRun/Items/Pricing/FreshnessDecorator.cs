namespace RpgRoguelikeRun.Items.Pricing;

public class FreshnessDecorator : PriceDecorator
{
    public FreshnessDecorator(IPriceModifier inner) : base(inner) { }

    public override int GetPrice(Item item)
    {
        int basePrice = _inner.GetPrice(item);

        if (!item.IsPerishable) return basePrice;

        double usedFraction = (double)item.DaysInStorage / item.ShelfLifeDays!.Value;

        double multiplier = usedFraction switch
        {
            < 0.5  => 1.0,
            < 0.8  => 0.85,
            < 1.0  => 0.60,
            _      => 0.25
        };

        return (int)Math.Round(basePrice * multiplier);
    }
}