using UnityEngine;

public class KickstartDataStructures
{
    public enum GroundType
    {
        Soil,
        Water,
        Solid
    }

    public enum ShadeRequirement
    {
        NeedsShade,
        NoShade,
        None
    }

    public enum ObjectType
    {
        LevelBorder,
        Plant,
        Decoration,
        Path,
        Grass
    }
}

[System.Serializable]
public class GroundTile
{
    // What type of tile is this?
    public KickstartDataStructures.GroundType type = KickstartDataStructures.GroundType.Soil;

    // Can tiles be placed on this? (Overrides groundType requirements)
    public bool placeable = true;

    // Is there shade on this tile?
    public bool isShaded = false;

}
