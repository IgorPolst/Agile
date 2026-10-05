using RpgRoguelikeRun.WorldLayer.Roads;
using RpgRoguelikeRun.WorldLayer.Roads.Safety;
using Xunit;

namespace RpgRoguelikeRun.Tests;

public class SafetyDecoratorTests
{
    // ================================================================
    // 1. BaseSafety возвращает чистую Safety
    // ================================================================
    [Fact]
    public void BaseSafety_ReturnsRawSafety()
    {
        
        var road = new RoyalHighway(length: 10);   // Safety = 0.85
        ISafetyModifier chain = new BaseSafety();

        
        double safety = chain.GetSafety(road);

        
        Assert.Equal(0.85, safety, precision: 2);
    }

    // ================================================================
    // 2. Quality — Paved не меняет
    // ================================================================
    [Fact]
    public void QualityDecorator_Paved_DoesNotChangeSafety()
    {
        
        var road = new RoyalHighway(length: 10);   // Quality = Paved
        ISafetyModifier chain = new QualitySafetyDecorator(new BaseSafety());

        
        double safety = chain.GetSafety(road);

        
        Assert.Equal(0.85, safety, precision: 2);
    }

    // ================================================================
    // 3. Quality — Muddy ×0.65
    // ================================================================
    [Fact]
    public void QualityDecorator_Muddy_MultipliesBy065()
    {
        
        var road = new RoyalHighway(length: 10);
        road.ApplyStorm();   // Paved → Dirt
        road.ApplyStorm();   // Dirt → Muddy
        ISafetyModifier chain = new QualitySafetyDecorator(new BaseSafety());

        
        double safety = chain.GetSafety(road);

        
        Assert.Equal(0.85 * 0.65, safety, precision: 2);
    }

    // ================================================================
    // 4. BanditChance = 1 - EffectiveSafety
    // ================================================================
    [Fact]
    public void BanditChance_EqualsOneMinusEffectiveSafety()
    {
        
        var road = new ForestPath(length: 6);   // Safety = 0.55, Quality = Dirt

        
        double bandit = road.BanditChance;
        double expected = 1.0 - road.EffectiveSafety;

        
        Assert.Equal(expected, bandit, precision: 2);
    }

    // ================================================================
    // 5. Заброшенная дорога → самая низкая безопасность
    // ================================================================
    [Fact]
    public void AbandonedRoad_HasLowestSafety()
    {
        
        var royal = new RoyalHighway(length: 10);
        var forest = new ForestPath(length: 6);
        var abandoned = new AbandonedRoad(length: 15);

        
        double safetyRoyal = royal.EffectiveSafety;
        double safetyForest = forest.EffectiveSafety;
        double safetyAbandoned = abandoned.EffectiveSafety;

        
        Assert.True(safetyRoyal > safetyForest,
            $"Royal {safetyRoyal} should be > forest {safetyForest}");
        Assert.True(safetyForest > safetyAbandoned,
            $"Forest {safetyForest} should be > abandoned {safetyAbandoned}");
    }

    // ================================================================
    // 6. Порядок: цепочка возвращает корректное значение
    // ================================================================
    [Fact]
    public void Chain_BaseAndQuality_ResultIsProduct()
    {
        
        var road = new ForestPath(length: 6);   // Safety = 0.55, Quality = Dirt
        double expected = 0.55 * 0.90;          // = 0.495

        
        ISafetyModifier chain = new QualitySafetyDecorator(new BaseSafety());
        double actual = chain.GetSafety(road);

        
        Assert.Equal(expected, actual, precision: 3);
    }
}