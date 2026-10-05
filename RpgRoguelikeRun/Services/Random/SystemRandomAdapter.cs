namespace RpgRoguelikeRun.Services.Random;

public class SystemRandomAdapter : IRandomProvider
{
    private readonly System.Random _random;

    public SystemRandomAdapter()
        : this(new System.Random()) { }

    public SystemRandomAdapter(int seed)
        : this(new System.Random(seed)) { }

    public SystemRandomAdapter(System.Random random)
    {
        _random = random ?? throw new ArgumentNullException(nameof(random));
    }

    public int Next(int min, int max) => _random.Next(min, max);
    public double NextDouble() => _random.NextDouble();
}