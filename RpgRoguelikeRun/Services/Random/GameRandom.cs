namespace RpgRoguelikeRun.Services.Random;

public static class GameRandom
{
    private static IRandomProvider _provider = new SystemRandomAdapter();

    public static IRandomProvider Provider
    {
        get => _provider;
        set => _provider = value ?? throw new ArgumentNullException(nameof(value));
    }

    public static void SetSeed(int seed)
    {
        _provider = new SeededRandomAdapter(seed);
    }

    public static void Reset()
    {
        _provider = new SystemRandomAdapter();
    }

    public static int Next(int min, int max) => _provider.Next(min, max);
    public static double NextDouble() => _provider.NextDouble();
}