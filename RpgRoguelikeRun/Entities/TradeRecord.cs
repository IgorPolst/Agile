namespace RpgRoguelikeRun.Entities;
public class TradeRecord
{
    public string ItemName { get; }
    public int Quantity { get; }
    public int PricePerUnit { get; }
    public int TotalPrice { get; }
    public bool IsPurchase { get; }

    public TradeRecord(string itemName, int quantity, int pricePerUnit, bool isPurchase)
    {
        ItemName = itemName;
        Quantity = quantity;
        PricePerUnit = pricePerUnit;
        TotalPrice = pricePerUnit * quantity;
        IsPurchase = isPurchase;
    }

    public override string ToString()
        => IsPurchase
            ? $"🛒 Куплено: {ItemName} x{Quantity} по {PricePerUnit} = {TotalPrice}"
            : $"💰 Продано: {ItemName} x{Quantity} по {PricePerUnit} = {TotalPrice}";
}