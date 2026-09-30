namespace RpgRoguelikeRun.WorldLayer;

public class WorldMap
{
    public int Width { get; private set; }
    public int Height { get; private set; }

    public WorldMap(int width, int height)
    {
        Width = width;
        Height = height;
    }
}