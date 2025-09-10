using UnityEngine;

public class PlantData : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string plantName = "undefined";
    public KickstartDataStructures.Color plantColor;

    public KickstartDataStructures.GroundType[] groundType;
    public KickstartDataStructures.ShadeRequirement shadeRequirement = KickstartDataStructures.ShadeRequirement.none;
    public bool needsWall = false;
    public KickstartDataStructures.GroundType needsAdjacentGround = KickstartDataStructures.GroundType.none;
    public KickstartDataStructures.GroundType noAdjacentGround = KickstartDataStructures.GroundType.none;
    public string[] needsAdjacentObject;
    public string[] noAdjacentObject;

}
