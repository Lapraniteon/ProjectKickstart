using UnityEngine;

public class KickstartDataStructures
{
    public enum GroundType
    {
        none,
        Soil,
        Water,
        Solid
    }

    public enum ShadeRequirement
    {
        none,
        NeedsShade,
        NoShade
    }

    public enum ObjectType
    {
        LevelBorder,
        Plant,
        Decoration,
        Path,
        Grass
    }

    public enum Color
    {
        undefined,
        White,
        Yellow,
        Red, 
        Green,
        Orange,
        Pink
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
