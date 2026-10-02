using RpgRoguelikeRun.Items;

namespace RpgRoguelikeRun.WorldLayer;

public class MarketLot
{
    public Item Item { get; }
    public int Quantity { get; set; }
    public int BasePricePerUnit { get; }

    public int TradedVolume { get; private set; }

    public MarketLot(Item item, int quantity, int pricePerUnit)
    {
        if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));
        if (pricePerUnit < 0) throw new ArgumentOutOfRangeException(nameof(pricePerUnit));

        Item = item;
        Quantity = quantity;
        BasePricePerUnit = pricePerUnit;
    }

    public void RegisterTrade(int quantity)
    {
        TradedVolume += quantity;
    }

    public override string ToString()
        => $"{Item.Name} x{Quantity}";
}