namespace RpgRoguelikeRun.Entities.Events;
public class WanderingKnight : RoadEvent
{
    public override string Title => "Странствующий рыцарь";
    public override bool IsFriendly => true;

    public int Cost { get; init; } = 20;

    public override void Trigger(Trader trader)
    {
        Console.WriteLine($"🛡️  {Title}: предлагает защиту за {Cost} золотых.");
        Console.WriteLine($"   [Y] Нанять, [N] Отказаться");

        var key = Console.ReadKey(true).Key;
        if (key != ConsoleKey.Y)
        {
            Console.WriteLine("   Вы отказались.");
            return;
        }

        if (trader.Gold < Cost)
        {
            Console.WriteLine("   Не хватает золота.");
            return;
        }

        trader.LoseGold(Cost);
        trader.ApplyBanditChanceBonus(-0.20);   // -20% к шансу нападения
        Console.WriteLine($"   Рыцарь защищает вас! Шанс нападения снижен.");
    }
}