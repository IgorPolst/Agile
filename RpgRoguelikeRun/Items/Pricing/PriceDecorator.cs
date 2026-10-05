using RpgRoguelikeRun.Items;

namespace RpgRoguelikeRun.Items.Pricing;

public abstract class PriceDecorator : IPriceModifier
{
    protected readonly IPriceModifier _inner;

    protected PriceDecorator(IPriceModifier inner)
    {
        _inner = inner ?? throw new ArgumentNullException(nameof(inner));
    }
    
    public virtual int GetPrice(Item item) => _inner.GetPrice(item);
}