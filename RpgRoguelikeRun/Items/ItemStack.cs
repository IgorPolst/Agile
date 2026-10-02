namespace RpgRoguelikeRun.Items;

public class ItemStack
{
    public Item Item { get; }
    public int Quantity { get; set; }

    public ItemStack(Item item, int quantity = 1)
    {
        if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));
        Item = item;
        Quantity = quantity;
    }

    public int TotalCost => Item.CurrentCost * Quantity;

    public override string ToString()
        => $"{Item.Name} x{Quantity} (по {Item.CurrentCost}, всего {TotalCost})";
}