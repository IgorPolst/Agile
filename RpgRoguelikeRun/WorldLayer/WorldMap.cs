namespace RpgRoguelikeRun.WorldMap;

public class WorldMap
{
    public int Width { get; set; }
    public int Height { get; set; }

    public WorldMap(int width, int height)
    {
        Width = width;
        Height = height;
    }
}