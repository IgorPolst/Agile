using RpgRoguelikeRun.Items;

namespace RpgRoguelikeRun.Items.Pricing;

public static class PricePipeline
{
    public static IPriceModifier ForItem(Item item)
    {
        IPriceModifier chain = new BasePrice();
        chain = new QualityDecorator(chain);
        chain = new RarityDecorator(chain);
        chain = new FreshnessDecorator(chain);
        return chain;
    }

    public static int CalculatePrice(Item item) => ForItem(item).GetPrice(item);
}