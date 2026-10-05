using RpgRoguelikeRun.Enums;

namespace RpgRoguelikeRun.Items.Pricing;

public class RarityDecorator : PriceDecorator
{
    public RarityDecorator(IPriceModifier inner) : base(inner) { }

    public override int GetPrice(Item item)
    {
        int basePrice = _inner.GetPrice(item);

        double multiplier = item.Rarity switch
        {
            ItemRarity.Common    => 1.0,
            ItemRarity.Uncommon  => 1.15,
            ItemRarity.Rare      => 1.40,
            ItemRarity.Legendary => 2.00,
            _                    => 1.0
        };

        return (int)Math.Round(basePrice * multiplier);
    }
}