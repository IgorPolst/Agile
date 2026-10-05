using RpgRoguelikeRun.WorldLayer.Roads;

using Xunit;

namespace RpgRoguelikeRun.Tests;

public class RoadTests
{
    // ================================================================
    // 1. ПОЗИТИВНЫЙ: Paved качество не снижает безопасность
    // ================================================================
    [Fact]
    public void EffectiveSafety_Paved_EqualsSafety()
    {
        var road = new RoyalHighway(length: 10);

        double safety = road.EffectiveSafety;

        Assert.Equal(0.85, safety, precision: 2);
    }

    // ================================================================
    // 2. ГРАНИЧНЫЙ: Muddy снижает безопасность до 65%
    // ================================================================
    [Fact]
    public void EffectiveSafety_Muddy_ReducesBy35Percent()
    {
        var road = new ForestPath(length: 5);
        road.ApplyStorm();

        double safety = road.EffectiveSafety;

        Assert.Equal(0.55 * 0.65, safety, precision: 2);
    }

    // ================================================================
    // 3. BanditChance = 1 - EffectiveSafety
    // ================================================================
    [Fact]
    public void BanditChance_EqualsOneMinusSafety()
    {
        var road = new RoyalHighway(length: 10);

        double bandit = road.BanditChance;

        Assert.Equal(1.0 - road.EffectiveSafety, bandit, precision: 2);
    }

    // ================================================================
    // 4. ГРАНИЧНЫЙ: TravelTime с плохим качеством больше
    // ================================================================
    [Fact]
    public void TravelTime_MuddyQuality_LongerThanPaved()
    {
        var paved = new RoyalHighway(length: 10);
        var muddy = new RoyalHighway(length: 10);
        muddy.ApplyStorm();
        muddy.ApplyStorm();

        int pavedTime = paved.TravelTime;
        int muddyTime = muddy.TravelTime;

        Assert.True(muddyTime > pavedTime,
            $"Muddy time {muddyTime} should be > paved time {pavedTime}");
    }

    // ================================================================
    // 5. ГРАНИЧНЫЙ: TravelTime минимум 1
    // ================================================================
    [Fact]
    public void TravelTime_TinyLength_AtLeastOne()
    {
        var road = new ForestPath(length: 1);

        int time = road.TravelTime;

        Assert.True(time >= 1);
    }
}