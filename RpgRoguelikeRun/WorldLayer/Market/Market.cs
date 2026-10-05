using RpgRoguelikeRun.Items;

namespace RpgRoguelikeRun.WorldLayer;

public class Market
{
    public List<MarketLot> Lots { get; } = new();
    public double BuybackRate { get; set; } = 0.8;
    public double Spread { get; set; } = 0.20;

    // ---------- Лоты ----------

    public void AddLot(Item item, int quantity, int pricePerUnit)
        => Lots.Add(new MarketLot(item, quantity, pricePerUnit));


    public MarketLot? FindLot(Item item)
        => Lots.FirstOrDefault(l =>
            l.Item.Name == item.Name &&
            l.Item.Category == item.Category &&
            l.Item.Quality == item.Quality &&
            l.Item.Rarity == item.Rarity);

    // ---------- Цены ----------

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
            : (int)Math.Round(item.CurrentCost * BuybackRate);

        return Math.Max(1, basePrice);
    }

    private int GetLotSellPrice(MarketLot lot)
    {
        int shift = lot.TradedVolume / 10;
        return Math.Max(1, lot.BasePricePerUnit - shift);
    }
}