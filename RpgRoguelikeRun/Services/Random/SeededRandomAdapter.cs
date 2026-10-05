namespace RpgRoguelikeRun.Services.Random;
public class SeededRandomAdapter : IRandomProvider
{
    private readonly System.Random _random;

    public SeededRandomAdapter(int seed)
    {
        _random = new System.Random(seed);
    }

    public int Next(int min, int max) => _random.Next(min, max);
    public double NextDouble() => _random.NextDouble();
}