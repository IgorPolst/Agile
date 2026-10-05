using RpgRoguelikeRun.Services.Random;


namespace RpgRoguelikeRun.Entities.Events;

public class BanditAmbush : RoadEvent
{
    public override string Title => "Засада разбойников";
    public override bool IsHostile => true;
    public int Damage { get; init; } = 50;
    public double DamageVariance { get; init; } = 0.3;
    public double StealChance { get; init; } = 0.5;
    public int MaxStolenItems { get; init; } = 2;

    public override void Trigger(Trader trader)
    {
        Console.WriteLine($"⚔️  {Title}:");

        double min = Damage * (1 - DamageVariance);
        double max = Damage * (1 + DamageVariance);
        int actualDamage = (int)Math.Round(min + GameRandom.NextDouble() * (max - min));
        int stolenGold = trader.LoseGold(actualDamage);
        Console.WriteLine($"   💰 Отняли {stolenGold} золота. Осталось: {trader.Gold}");

        if (trader.Inventory.Stacks.Count == 0) return;

        if (GameRandom.NextDouble() > StealChance)
        {
            Console.WriteLine("   🎒 Товар не тронули.");
            return;
        }

        int stolenCount = GameRandom.Next(1, MaxStolenItems + 1);
        var stolen = new List<string>();

        for (int i = 0; i < stolenCount; i++)
        {
            var stacks = trader.Inventory.Stacks;
            if (stacks.Count == 0) break;

            var stack = stacks[GameRandom.Next(0, stacks.Count)];
            string name = stack.Item.Name;
            int qty = 1;

            if (trader.Inventory.Remove(stack.Item, qty))
                stolen.Add($"{name} x{qty}");
        }

        if (stolen.Count > 0)
            Console.WriteLine($"   🎒 Отняли товар: {string.Join(", ", stolen)}");
        else
            Console.WriteLine("   🎒 Товар отнять не удалось.");
    }
}