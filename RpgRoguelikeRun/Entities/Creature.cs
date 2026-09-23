namespace RpgRoguelikeRun.Entities;
public abstract class Creature
{
    public string Name { get; set; }
    public int Gold { get; set; }

    protected Creature(string name, int gold = 0)
    {
        Name = name;
        Gold = gold;
    }

    public virtual void Move()
    {
        // Базовая логика перемещения — переопределяется в наследниках
    }
}