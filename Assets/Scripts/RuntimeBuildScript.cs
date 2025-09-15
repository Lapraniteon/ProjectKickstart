using UnityEngine;
using UnityEngine.Rendering;

public class RuntimeBuildScript : MonoBehaviour
{
    public ObjectTile[] PrefabList;
    public int selectedPrefabIndex = 0; // should move this to the game controller probably

    //the "coordinates" variable here is in array orientation, so world position has x and y reversed. 
    public void PlaceObject(Vector2Int coordinates)
    {
        //instantiate prefab at coordinates.
        //viable check:
        // if no, destroy
        // if yes, put in array

        if (selectedPrefabIndex == 0)
        {
            Debug.Log("No plant selected.");
        } else
        {
            ObjectTile newObject = Instantiate(PrefabList[selectedPrefabIndex], new Vector3(coordinates.y+0.5f, 0f, -coordinates.x+0.5f), Quaternion.identity);
            if (!RequirementCheck(newObject, coordinates))
            {
                Destroy(newObject);
            } else
            {
                GameManager.Instance.gameGrid.AddToObjectArray(newObject, coordinates);
            }
        }


    }

    public void SelectPlant(int index)
    {
        selectedPrefabIndex = index;
    }

    bool RequirementCheck(ObjectTile obj, Vector2Int coordinates)
    {
        ObjectTile[,] objArray = GameManager.Instance.gameGrid.objectArray;
        GroundTile[,] groundArray = GameManager.Instance.gameGrid.groundArray;

        int objRow = coordinates.x;
        int objCol = coordinates.y;

        // check if spot is empty
        if (objArray[objRow, objCol] != null) return false;

        //check for correct type of soil
        bool correctGround = false;
        foreach (KickstartDataStructures.GroundType plantable in obj.groundType)
        {
            if (plantable == groundArray[objRow, objCol].type)
            {
                correctGround = true;
                break;
            }
        }
        if (!correctGround) return false;

        // check shade requirement -> 1 for ground, 1 for shade providing plants. ground OR plant is true -> shade. 
        bool targetIsShaded = false;
        //why exactly aren't we storing all shaded tiles in the grid? -> coding when to remove shade from array and when not to is hell. 
        if (groundArray[objRow, objCol].isShaded) targetIsShaded = true;
        if (GameManager.Instance.gameGrid.ShadeProvidingPlantsNextToCell(objRow, objCol)) targetIsShaded = true;
        
        if ((obj.shadeRequirement == KickstartDataStructures.ShadeRequirement.NeedsShade && targetIsShaded == false) ||
            (obj.shadeRequirement == KickstartDataStructures.ShadeRequirement.NoShade && targetIsShaded == true))
        {
            return false;
        }

        // check soil adjacency requirements
        foreach (KickstartDataStructures.GroundType groundType in GameManager.Instance.gameGrid.AdjacentGroundToCell(objRow, objCol))
        {
            if (obj.noAdjacentGround != KickstartDataStructures.GroundType.none && obj.noAdjacentGround == groundType)
            {
                return false;
            }

            if (obj.needsAdjacentGround != KickstartDataStructures.GroundType.none && obj.needsAdjacentGround != groundType)
            {
                return false;
            }
        }

        // check plant adjacency requirements
        foreach (string listName in GameManager.Instance.gameGrid.AdjacentObjectsToCell(objRow, objCol))
        {
            if (obj.noAdjacentObject != null)
            {
                foreach (string forbidden in obj.noAdjacentObject)
                {
                    if (forbidden == listName) return false;
                }
            }
            if (obj.needsAdjacentObject != null)
            {
                foreach (string required in obj.needsAdjacentObject)
                {
                    if (required != listName) return false;
                }
            }
        }

        return true;
    }
}
