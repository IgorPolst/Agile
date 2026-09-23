using RpgRoguelikeRun.Entities;

namespace RpgRoguelikeRun.WorldMap;
public class World
{
    public WorldMap Map { get; }
    public Trader? Trader { get; private set; }
    public List<Enemy> Enemies { get; } = new();

    public World(int width, int height)
    {
        Map = new WorldMap(width, height);
    }

    public void AddTrader(Trader trader)
    {
        Trader = trader;
    }

    public void AddEnemy(Enemy enemy)
    {
        Enemies.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        Enemies.Remove(enemy);
    }
}