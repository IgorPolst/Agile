using RpgRoguelikeRun.Services.Random;
using Xunit;

namespace RpgRoguelikeRun.Tests;

public class RandomAdapterTests
{
    // ================================================================
    // 1. SeededRandomAdapter даёт одинаковые числа
    // ================================================================
    [Fact]
    public void SeededAdapter_SameSeed_ProducesSameNumbers()
    {
        var a = new SeededRandomAdapter(seed: 42);
        var b = new SeededRandomAdapter(seed: 42);

        int a1 = a.Next(1, 1000);
        int b1 = b.Next(1, 1000);

        Assert.Equal(a1, b1);
    }

    // ================================================================
    // 2. Разные seed → разные числа
    // ================================================================
    [Fact]
    public void SeededAdapter_DifferentSeeds_ProduceDifferentNumbers()
    {
        var a = new SeededRandomAdapter(seed: 42);
        var b = new SeededRandomAdapter(seed: 99);
        
        int a1 = a.Next(1, 1_000_000);
        int b1 = b.Next(1, 1_000_000);

        
        Assert.NotEqual(a1, b1);
    }

    // ================================================================
    // 3. Next(min, max) в диапазоне
    // ================================================================
    [Fact]
    public void Next_WithinRange()
    {
        var random = new SystemRandomAdapter();

        for (int i = 0; i < 100; i++)
        {
            int value = random.Next(10, 20);
            Assert.InRange(value, 10, 19);
        }
    }

    // ================================================================
    // 4. NextDouble в диапазоне [0, 1)
    // ================================================================
    [Fact]
    public void NextDouble_WithinZeroOne()
    { 
        var random = new SystemRandomAdapter();

        for (int i = 0; i < 100; i++)
        {
            double value = random.NextDouble();
            Assert.InRange(value, 0.0, 1.0);
        }
    }
}