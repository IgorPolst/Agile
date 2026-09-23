namespace RpgRoguelikeRun.Items;

public class Inventory
{
    public int Capacity { get; set; }

    public List<Item> Items { get; } = new();

    public Inventory(int capacity)
    {
        Capacity = capacity;
    }

    public bool Add(Item item, int quantity = 1)
    {
        if (Items.Count + quantity > Capacity)
            return false;

        for (int i = 0; i < quantity; i++)
            Items.Add(item);

        return true;
    }

    public bool Remove(Item item)
    {
        return Items.Remove(item);
    }

    public int Count => Items.Count;
}