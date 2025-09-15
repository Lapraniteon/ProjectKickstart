using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class GameGrid : MonoBehaviour
{
    public int width;
    public int height;

    public ObjectTile dummyTilePrefab;
    
    public GroundTile[,] groundArray;
    public ObjectTile[,] objectArray;
    
    public BuildLevel buildLevel;

    void Awake()
    {
        groundArray = new GroundTile[height, width];
        
        objectArray = new ObjectTile[height, width];

        //objectArray[0,0] = Instantiate(dummyTilePrefab);
        //objectArray[0,0].objectType = KickstartDataStructures.ObjectType.Plant;
        //objectArray[0, 0].objectName = "Lily";
        //objectArray[0, 0].color = KickstartDataStructures.Color.White;
        
        //CountObjectsWithTag("Lily", KickstartDataStructures.ObjectType.Plant);
        //CountObjectsWithTag(KickstartDataStructures.Color.White, KickstartDataStructures.ObjectType.Plant);
        //CountAmountOfPlantColors();
    }

    void Start()
    {
        populateGroundArray();
        
        buildLevel.BuildLevelObjects();
        Debug.Log("Finish building objects");
    }

    //fills the ground array with data from a list in Level Layout Data. Should probably be generalised. 
    void populateGroundArray()
    {
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                groundArray[row, col] = new GroundTile
                {
                    type = (KickstartDataStructures.GroundType)GameManager.Instance.levelLayoutData.level1GroundType[row, col]
                };
                
                if (col == width-1)
                {
                    groundArray[row, col].isShaded = true;
                }
            }
        }
    }

    //count all objects of a type with a specific name
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

    //count all objects of a type with a specific color
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

    //counts all unique colors in the grid.
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
    
    //checks the entire grid for any occurrences of two specific adjacent objects, or one object and another object of any colour
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

    //if any adjacent cell to the given position is a shade giving plant, return true. Otherwise return false. 
    public bool ShadeProvidingPlantsNextToCell (int row, int col)
    {
        /*foreach (Vector2Int point in surroundingPoints)
        {
            if (objectArray[row + point.x, col + point.y].providesShade)
                return true;
        }*/
        return false;
    }

    public void AddToObjectArray(ObjectTile obj, Vector2Int location)
    {
        objectArray[location.x, location.y] = obj;
    }

    //checks adjacent cells to a given position and returns a list of all (non-zero) ground type values.
    public List<KickstartDataStructures.GroundType> AdjacentGroundToCell(int row, int col)
    {
        List<KickstartDataStructures.GroundType> adjacencyList = new List<KickstartDataStructures.GroundType>();

        foreach (Vector2Int point in surroundingPoints)
        {
            if (groundArray[row, col].type != KickstartDataStructures.GroundType.none)
            {
                adjacencyList.Add(groundArray[row, col].type);
            }
           
        }

        return adjacencyList;
    }

    public List<String> AdjacentObjectsToCell(int row, int col)
    {
        List<String> adjacencyList = new List<String>();

        foreach (Vector2Int point in surroundingPoints)
        {
            if (objectArray[row, col] != null)
            {
                adjacencyList.Add(objectArray[row, col].name);
            }

        }

        return adjacencyList;
    }

}
