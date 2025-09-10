using UnityEngine;

public class GameGrid : MonoBehaviour
{
    public int width;
    public int height;
    
    public GroundTile[,] groundArray;
    public ObjectTile[,] objectArray;

    void Awake()
    {
        groundArray = new GroundTile[height, width];
        
        objectArray = new ObjectTile[height, width];
    }

}
