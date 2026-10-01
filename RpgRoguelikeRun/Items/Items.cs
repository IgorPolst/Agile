using System.Diagnostics.CodeAnalysis;

namespace RpgRoguelikeRun.Items;

public class Item
{
    public string Name { get; set; }
    public int Cost { get; set; }
    public string Category { get; set; }
    public string Rarity { get; set; }

    public string Feature {get; set; }

    public Item(string name, int cost, string category, string rarity, string feature = null!)
    {
        Name = name;
        Cost = cost;
        Category = category;
        Rarity = rarity;
        Feature = feature;
    }

    public void Take()
    {
        // Подобрать предмет
    }

    public void Remove()
    {
        // Убрать предмет
    }
}