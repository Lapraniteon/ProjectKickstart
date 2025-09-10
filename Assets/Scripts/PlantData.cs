using UnityEngine;

public class PlantData : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string plantName = "undefined";
    public KickstartDataStructures.Color plantColor;

    public KickstartDataStructures.GroundType[] groundType;
    public KickstartDataStructures.ShadeRequirement shadeRequirement;
    public bool needsWall = false;
    public KickstartDataStructures.GroundType needsAdjacentGround;
    public KickstartDataStructures.GroundType noAdjacentGround;
    public string[] needsAdjacentObject;
    public string[] noAdjacentObject;

}
