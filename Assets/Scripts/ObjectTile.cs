using UnityEngine;

public class ObjectTile : MonoBehaviour
{
    [Tooltip("Can this object be moved or removed?")]
    public bool canBeEdited = true;

    [Tooltip("What type of object is this?")]
    public KickstartDataStructures.ObjectType objectType;
    
    public string objectName = "undefined";
    public KickstartDataStructures.Color color;

    public KickstartDataStructures.GroundType[] groundType;
    public KickstartDataStructures.ShadeRequirement shadeRequirement = KickstartDataStructures.ShadeRequirement.none;
    public bool needsWall = false;
    public KickstartDataStructures.GroundType needsAdjacentGround = KickstartDataStructures.GroundType.none;
    public KickstartDataStructures.GroundType noAdjacentGround = KickstartDataStructures.GroundType.none;
    public string[] needsAdjacentObject;
    public string[] noAdjacentObject;
    public bool providesShade = false;
}
