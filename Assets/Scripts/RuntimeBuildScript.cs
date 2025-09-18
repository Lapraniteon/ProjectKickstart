using DG.Tweening;
using System.Collections.Generic;
using System;
using Mono.Cecil;
using UnityEngine;
using UnityEngine.Rendering;

public class RuntimeBuildScript : MonoBehaviour
{
    public ObjectTile[] PrefabList;
    public int selectedPrefabIndex = 0; // should move this to the game controller probably
    
    string feedbackToReturn = "default";

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
            if (selectedPrefabIndex >= PrefabList.Length)
            {
                Debug.LogWarning($"No plant assigned in array for index {selectedPrefabIndex}");
                return;
            }
            
            ObjectTile newObject = Instantiate(PrefabList[selectedPrefabIndex], new Vector3(coordinates.y+0.5f, 0f, -coordinates.x-0.5f), Quaternion.identity);

            (bool, string) requirementCheckFulfilled = RequirementCheck(newObject, coordinates);
            
            if (!requirementCheckFulfilled.Item1)
            {
                Destroy(newObject.gameObject);
                GameManager.Instance.uiController.FlashCantPlaceIndicator(requirementCheckFulfilled.Item2);
            } else
            {
                GameManager.Instance.gameGrid.AddToObjectArray(newObject, coordinates);
                Debug.Log("Placed object");

                newObject.transform.localScale = Vector3.zero;
                newObject.transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
            }
            
        }


    }

    public void DeleteObject(Vector2Int coordinates)
    {
        if (GameManager.Instance.gameGrid.objectArray[coordinates.x, coordinates.y] == null)
            return;

        if (GameManager.Instance.gameGrid.objectArray[coordinates.x, coordinates.y].canBeEdited == false)
            return;
        
        GameObject objectToDestroy = GameManager.Instance.gameGrid.objectArray[coordinates.x, coordinates.y].gameObject;
        GameManager.Instance.gameGrid.objectArray[coordinates.x, coordinates.y] = null;
        
        objectToDestroy.transform.DOScale(0, 0.2f).SetEase(Ease.InBack).onComplete = () => Destroy(objectToDestroy);
    }

    public void SelectPlant(int index)
    {
        GameManager.Instance.SetDeleteMode(false);
        GameManager.Instance.uiController.SetDeleteModePanel(false);
        selectedPrefabIndex = index;
    }

    (bool, string) RequirementCheck(ObjectTile obj, Vector2Int coordinates)
    {
        
        ObjectTile[,] objArray = GameManager.Instance.gameGrid.objectArray;
        GroundTile[,] groundArray = GameManager.Instance.gameGrid.groundArray;

        int objRow = coordinates.x;
        int objCol = coordinates.y;

        // check if spot is empty
        if (!EmptyCheck(obj, objArray, objRow, objCol)) return (false, feedbackToReturn); // Spot is occupied

        //check for correct type of soil
        if (!CorrectGround(obj, groundArray, objRow, objCol)) return (false, feedbackToReturn); // Wrong type of ground

        // check shade requirement -> 1 for ground, 1 for shade providing plants. ground OR plant is true -> shade. 
        if (!ShadeReqMet(obj, groundArray, objRow, objCol)) return (false, feedbackToReturn); // 

        // check soil adjacency requirements
        if (!SoilAdjMet(obj, groundArray, objRow, objCol)) return (false, feedbackToReturn);

        // check plant adjacency requirements
        if (!ObjAdjMet(obj, objRow, objCol)) return (false, feedbackToReturn);

        return (true, "irrelevant");
    }

    bool EmptyCheck(ObjectTile obj, ObjectTile[,] objArray, int objRow, int objCol)
    {
        if (objArray[objRow, objCol] != null)
        {
            Debug.Log("Location occupied");
            feedbackToReturn = "Tile is already occupied.";
            return false;
        }
        
        return true;
    }

    bool CorrectGround(ObjectTile obj, GroundTile[,] groundArray, int objRow, int objCol)
    {
        foreach (KickstartDataStructures.GroundType plantable in obj.groundType)
        {
            if (plantable == groundArray[objRow, objCol].type)
            {
                return true;
            }
        }
        
        Debug.Log("Wrong ground type");
        feedbackToReturn = "Wrong type of terrain.";
        return false;
    }

    bool ShadeReqMet(ObjectTile obj, GroundTile[,] groundArray, int objRow, int objCol)
    {
        bool targetIsShaded = groundArray[objRow, objCol].isShaded;
        //why exactly aren't we storing all shaded tiles in the grid? -> coding when to remove shade from array and when not to is hell. 
        if (GameManager.Instance.gameGrid.ShadeProvidingPlantsNextToCell(objRow, objCol)) targetIsShaded = true;

        if (obj.shadeRequirement == KickstartDataStructures.ShadeRequirement.NeedsShade && !targetIsShaded)
        {
            Debug.Log("Shade requirement unmet");
            feedbackToReturn = "This plant needs to be in shade.";
            return false;
        }
        
        if (obj.shadeRequirement == KickstartDataStructures.ShadeRequirement.NoShade && targetIsShaded)
        {
            Debug.Log("Shade requirement unmet");
            feedbackToReturn = "This plant needs sunlight.";
            return false;
        }
        
        return true;
    }

    bool SoilAdjMet(ObjectTile obj, GroundTile[,] groundArray, int objRow, int objCol)
    {
        foreach (KickstartDataStructures.GroundType groundType in GameManager.Instance.gameGrid.AdjacentGroundToCell(objRow, objCol))
        {
            if (obj.noAdjacentGround != KickstartDataStructures.GroundType.none && obj.noAdjacentGround == groundType)
            {
                Debug.Log("Forbidden ground type nearby");
                feedbackToReturn = "This cannot be next to that terrain.";
                return false;
            }

            if (obj.needsAdjacentGround != KickstartDataStructures.GroundType.none && obj.needsAdjacentGround != groundType)
            {
                Debug.Log("Needed adjacent ground type missing");
                feedbackToReturn = "This needs to be next to different terrain.";
                return false;
            }
        }
        return true;
    }

    bool ObjAdjMet(ObjectTile obj, int objRow, int objCol)
    {
        // variables for needed object check
        List<string> discoveredNearby = new ();

        foreach (string listName in GameManager.Instance.gameGrid.AdjacentObjectsToCell(objRow, objCol))
        {
 
            if (obj.noAdjacentObject != null)
            {
                foreach (string forbidden in obj.noAdjacentObject)
                {
                    if (forbidden == listName)
                    {
                        Debug.Log("Forbidden adjacent object");
                        feedbackToReturn = "This cannot be next to that plant or object.";
                        return false;
                    }
                }
            }
            if (obj.needsAdjacentObject != null)
            {
                // if one or more needed objects exist: check if current object is needed -> check if is already discovered, if not, add and increase counter. 
                foreach (string required in obj.needsAdjacentObject)
                {
                    if (required == listName)
                    {
                        if (!discoveredNearby.Contains(required))
                        {
                            discoveredNearby.Add(required);
                        }
                    }
                }
            }
        }

        //for needed adjacents - check if number of discovered required adjacents is equal to number of required adjacents. 
        if (obj.needsAdjacentObject != null)
        {
            if (obj.needsAdjacentObject.Length != discoveredNearby.Count)
            {
                Debug.Log("Required adjacent missing");
                feedbackToReturn = "This needs to be next to a different object.";
                return false;
            }
        }

        return true;
    }
}
