using System;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    public int width;
    public int height;

    public ObjectTile dummyTilePrefab;
    
    public GroundTile[,] groundArray;
    public ObjectTile[,] objectArray;

    void Awake()
    {
        groundArray = new GroundTile[height, width];
        
        objectArray = new ObjectTile[height, width];

        objectArray[0,0] = Instantiate(dummyTilePrefab);
        objectArray[0,0].objectType = KickstartDataStructures.ObjectType.Plant;
        objectArray[0, 0].objectName = "Lily";
        objectArray[0, 0].color = KickstartDataStructures.Color.White;
        
        CountObjectsWithTag("Lily", KickstartDataStructures.ObjectType.Plant);
        CountObjectsWithTag(KickstartDataStructures.Color.White, KickstartDataStructures.ObjectType.Plant);
        CountAmountOfPlantColors();
    }

    public int CountObjectsWithTag(string objectName, KickstartDataStructures.ObjectType objectType)
    {
        int count = 0;
        foreach (ObjectTile obj in objectArray)
        {
            if (obj == null)
                continue;
            
            if (obj.objectName == objectName && obj.objectType == objectType)
                count++;
        }

        Debug.Log(count + " objects with name: " + objectName);
        return count;
    }

    public int CountObjectsWithTag(KickstartDataStructures.Color color, KickstartDataStructures.ObjectType objectType)
    {
        int count = 0;
        foreach (ObjectTile obj in objectArray)
        {
            if (obj == null)
                continue;
            
            if (obj.color == color && obj.objectType == objectType)
                count++;
        }

        Debug.Log(count + " objects with color: " + color);
        return count;
    }

    public int CountAmountOfPlantColors()
    {
        List<KickstartDataStructures.Color> discoveredColors = new();

        foreach (ObjectTile obj in objectArray)
        {
            if (obj == null)
                continue;
            
            if (obj.objectType != KickstartDataStructures.ObjectType.Plant)
                continue;
            
            if (!discoveredColors.Contains(obj.color))
                discoveredColors.Add(obj.color);
        }

        Debug.Log(discoveredColors.Count + " unique colors.");
        return discoveredColors.Count;
    }

    Vector2Int[] surroundingPoints =
    {
        new Vector2Int(1, 0),
        new Vector2Int(1, 1),
        new Vector2Int(0, 1),
        new Vector2Int(-1, 1),
        new Vector2Int(-1, 0),
        new Vector2Int(-1, -1),
        new Vector2Int(0, -1),
        new Vector2Int(1, -1),
    };
    
    public bool AreThereAdjacentObjects(string name1, string name2 = "", KickstartDataStructures.Color color2 = KickstartDataStructures.Color.undefined)
    {
        for (int row = 1; row < height - 1; row++)
        {
            for (int col = 1; col < width - 1; col++)
            {
                if (objectArray[row, col] == null)
                    continue;
                
                if (objectArray[row, col].objectName != name1) // Skip any objects that we dont want to check for
                    continue;

                foreach (Vector2Int point in surroundingPoints)
                {
                    if (objectArray[row + point.x, col + point.y].objectName == name2 && !String.IsNullOrEmpty(name2))
                        return true;

                    if (objectArray[row + point.x, col + point.y].color == color2 &&
                        !color2.Equals(KickstartDataStructures.Color.undefined))
                        return true;

                }
            }
        }

        return false;
    }

    public void Add(ObjectTile obj, Vector2Int location)
    {
        objectArray[location.y, location.x] = obj;
    }

}
