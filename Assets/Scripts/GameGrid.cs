using System.Collections.Generic;
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

    public int CountObjectsWithTag(string objectName, KickstartDataStructures.ObjectType objectType)
    {
        int count = 0;
        foreach (ObjectTile obj in objectArray)
        {
            if (obj.name == objectName && obj.objectType == objectType)
                count++;
        }

        return count;
    }

    public int CountObjectsWithTag(KickstartDataStructures.Color color, KickstartDataStructures.ObjectType objectType)
    {
        int count = 0;
        foreach (ObjectTile obj in objectArray)
        {
            if (obj.color == color && obj.objectType == objectType)
                count++;
        }

        return count;
    }

    public int CountAmountOfPlantColors()
    {
        List<KickstartDataStructures.Color> discoveredColors = new();

        foreach (ObjectTile obj in objectArray)
        {
            if (obj.objectType != KickstartDataStructures.ObjectType.Plant)
                continue;
            
            if (!discoveredColors.Contains(obj.color))
                discoveredColors.Add(obj.color);
        }
        
        return discoveredColors.Count;
    }

    /*public bool CheckIfThereAreAdjacentObjects(string name1, string name2 = "", KickstartDataStructures.Color color2 = KickstartDataStructures.Color.undefined)
    {
        
    }*/

}
