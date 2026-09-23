namespace RpgRoguelikeRun.Entities;

public class Enemy : Creature
{
    public Enemy(string name = "Враг", int gold = 0)
        : base(name, gold) { }

    public override void Move()
    {
        // Враги тоже двигаются по миру
    }

    public void Trade()
    {
        // На диаграмме у Enemy указан Trade() — оставляем как есть
    }
}