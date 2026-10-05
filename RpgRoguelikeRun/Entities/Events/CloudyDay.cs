namespace RpgRoguelikeRun.Entities.Events;

public class CloudyDay : RoadEvent
{
    public override string Title => "Пасмурный день";
    public override bool IsHostile => true;   // повышает шанс hostile

    public override void Trigger(Trader trader)
    {
        Console.WriteLine($"☁️  {Title}: мрачно, бандиты активизировались.");
        trader.ApplyBanditChanceBonus(0.15);   // +15% к шансу нападения
    }
}