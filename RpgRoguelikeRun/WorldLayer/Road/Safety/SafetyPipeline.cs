using RpgRoguelikeRun.WorldLayer.Roads;

namespace RpgRoguelikeRun.WorldLayer.Roads.Safety;

public static class SafetyPipeline
{
    /// <summary>
    /// Собрать цепочку: Base → Quality.
    /// Позже сюда легко добавить NightSafetyDecorator, WeatherSafetyDecorator.
    /// </summary>
    public static ISafetyModifier ForRoad(Road road)
    {
        ISafetyModifier chain = new BaseSafety();
        chain = new QualitySafetyDecorator(chain);
        return chain;
    }

    public static double CalculateEffectiveSafety(Road road)
        => ForRoad(road).GetSafety(road);
}