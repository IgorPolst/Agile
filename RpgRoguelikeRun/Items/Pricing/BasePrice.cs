namespace RpgRoguelikeRun.Items.Pricing;

public class BasePrice : IPriceModifier
{
    public int GetPrice(Item item) => item.BaseCost;
}
