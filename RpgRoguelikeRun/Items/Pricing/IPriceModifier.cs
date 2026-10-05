using RpgRoguelikeRun.Items;

namespace RpgRoguelikeRun.Items.Pricing;

public interface IPriceModifier
{
    int GetPrice(Item item);
}