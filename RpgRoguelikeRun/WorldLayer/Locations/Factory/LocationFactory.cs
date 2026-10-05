using RpgRoguelikeRun.WorldLayer.Regions;
namespace RpgRoguelikeRun.WorldLayer.Locations.Factory;
public abstract class LocationFactory
{
    public abstract Location Create(string name, IRegionStrategy region);
}