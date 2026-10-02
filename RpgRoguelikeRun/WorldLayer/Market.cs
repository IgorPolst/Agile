using RpgRoguelikeRun.Items;

namespace RpgRoguelikeRun.WorldLayer;

public class Market
{

    public List<MarketLot> Lots { get; } = new();
    public double BuybackRate { get; set; } = 0.8;
    public double Spread { get; set; } = 0.20;

    public Market() { }
    public void AddLot(Item item, int quantity, int pricePerUnit)
    {
        Lots.Add(new MarketLot(item, quantity, pricePerUnit));
    }

    public int GetBuyPrice(MarketLot lot)
    {
        int sellPrice = GetLotSellPrice(lot);
        int buyPrice = (int)Math.Round(sellPrice * (1 + Spread));
        return Math.Max(sellPrice + 1, buyPrice);
    }

    public int GetSellPrice(Item item, int quantity)
    {
        var lot = FindLot(item);
        int basePrice = lot != null
            ? GetLotSellPrice(lot)
            : (int)Math.Round(item.CurrentCost * 0.8);

        int totalSold = lot?.TradedVolume ?? 0;
        int priceShift = totalSold / 10;

        return Math.Max(1, basePrice - priceShift);
    }

     private int GetLotSellPrice(MarketLot lot)
    {
        int priceShift = lot.TradedVolume / 10;
        return Math.Max(1, lot.BasePricePerUnit - priceShift);
    }

    private MarketLot? FindLot(Item item)
        => Lots.FirstOrDefault(l =>
            l.Item.Name == item.Name &&
            l.Item.Category == item.Category &&
            l.Item.Quality == item.Quality);

        
}

public class MarketLot
{
    public Item Item { get; }
    public int Quantity { get; set; }
    public int BasePricePerUnit { get; }
    public int PricePerUnit { get; private set; }
    public int TradedVolume { get; private set; }

    public MarketLot(Item item, int quantity, int pricePerUnit)
    {
        if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));
        if (pricePerUnit < 0) throw new ArgumentOutOfRangeException(nameof(pricePerUnit));

        Item = item;
        Quantity = quantity;
        BasePricePerUnit = pricePerUnit;
        PricePerUnit = pricePerUnit;
    }

    public void RegisterTrade(int quantity, bool isSale)
    {
        TradedVolume += quantity;
        int shift = TradedVolume / 10;

        if (isSale)
            PricePerUnit = Math.Max(1, BasePricePerUnit - shift);
        else
            PricePerUnit = BasePricePerUnit + shift;    
    }

    public override string ToString()
        => $"{Item.Name} x{Quantity} — {PricePerUnit}/шт";
}