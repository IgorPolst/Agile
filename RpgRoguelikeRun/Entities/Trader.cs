using RpgRoguelikeRun.Items;

namespace RpgRoguelikeRun.Entities;

public class Trader : Creature
{
    public Inventory Inventory { get; }

    public Trader(string name, int gold = 0, int capacity = 20)
        : base(name, gold)
    {
        Inventory = new Inventory(capacity);
    }

    public override void Move()
    {
        // Торговец двигается по карте (караван, дороги и т.п.)
    }

    public void Trade()
    {
        // Логика купли-продажи на рынке
    }
}