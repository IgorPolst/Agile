namespace RpgRoguelikeRun.Items;

public class Inventory
{
    public int Capacity { get; set; }
    public List<ItemStack> Stacks { get; } = new();

    public Inventory(int capacity)
    {
        if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity));
        Capacity = capacity;
    }

    public int Count => Stacks.Sum(s => s.Quantity);
    public bool HasSpaceFor(int quantity) => Count + quantity <= Capacity;
    public bool Add(Item item, int quantity = 1)
    {
        if (quantity <= 0) return false;
        if (!HasSpaceFor(quantity)) return false;

        var existing = FindStack(item);
        if (existing != null)
        {
            existing.Quantity += quantity;
        }
        else
        {
            Stacks.Add(new ItemStack(item, quantity));
        }
        return true;
    }

    public bool Remove(Item item, int quantity = 1)
    {
        if (quantity <= 0) return false;

        var stack = FindStack(item);
        if (stack == null || stack.Quantity < quantity) return false;

        stack.Quantity -= quantity;
        if (stack.Quantity == 0)
            Stacks.Remove(stack);

        return true;
    }

        public void AgeItems(int days = 1)
    {
        foreach (var stack in Stacks)
            stack.Item.DaysInStorage += days;
    }

    public int CountOf(Item item)
        => FindStack(item)?.Quantity ?? 0;

    private ItemStack? FindStack(Item item)
        => Stacks.FirstOrDefault(s =>
            s.Item.Name == item.Name &&
            s.Item.Category == item.Category &&
            s.Item.Quality == item.Quality &&
            s.Item.Rarity == item.Rarity);
}