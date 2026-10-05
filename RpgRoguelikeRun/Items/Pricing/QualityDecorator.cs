using RpgRoguelikeRun.Items;

namespace RpgRoguelikeRun.Items.Pricing;

public class QualityDecorator : PriceDecorator
{
    public QualityDecorator(IPriceModifier inner) : base(inner) { }

    public override int GetPrice(Item item)
    {
        int basePrice = _inner.GetPrice(item);

        double multiplier = item.Quality switch
        {
            ItemQuality.Poor      => 0.7,
            ItemQuality.Common    => 1.0,
            ItemQuality.Good      => 1.4,
            ItemQuality.Excellent => 2.0,
            _                     => 1.0
        };

        return (int)Math.Round(basePrice * multiplier);
    }
}