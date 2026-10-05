namespace RpgRoguelikeRun.Services.Random;

public interface IRandomProvider
{

    int Next(int min, int max);

    double NextDouble();
}