using DG.Tweening;
using System.Collections.Generic;
using System;
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
            ObjectTile newObject = Instantiate(PrefabList[selectedPrefabIndex], new Vector3(coordinates.y+0.5f, 0f, -coordinates.x-0.5f), Quaternion.identity);
            if (!RequirementCheck(newObject, coordinates))
            {
                Destroy(newObject.gameObject);
            } else
            {
                GameManager.Instance.gameGrid.AddToObjectArray(newObject, coordinates);
                Debug.Log("Placed object");

                newObject.transform.localScale = Vector3.zero;
                newObject.transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
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
        if (!EmptyCheck(obj, objArray, objRow, objCol)) return false;

        //check for correct type of soil
        if (!CorrectGround(obj, groundArray, objRow, objCol)) return false;

        // check shade requirement -> 1 for ground, 1 for shade providing plants. ground OR plant is true -> shade. 
        if (!ShadeReqMet(obj, groundArray, objRow, objCol)) return false;

        // check soil adjacency requirements
        if (!SoilAdjMet(obj, groundArray, objRow, objCol)) return false;

        // check plant adjacency requirements
        if (!ObjAdjMet(obj, objRow, objCol)) return false;

        return true;
    }

    bool EmptyCheck(ObjectTile obj, ObjectTile[,] objArray, int objRow, int objCol)
    {
        if (objArray[objRow, objCol] != null)
        {
            Debug.Log("Location occupied");
            return false;
        }
        else return true;
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
        return false;
    }

    bool ShadeReqMet(ObjectTile obj, GroundTile[,] groundArray, int objRow, int objCol)
    {
        bool targetIsShaded = false;
        //why exactly aren't we storing all shaded tiles in the grid? -> coding when to remove shade from array and when not to is hell. 
        if (groundArray[objRow, objCol].isShaded) targetIsShaded = true;
        if (GameManager.Instance.gameGrid.ShadeProvidingPlantsNextToCell(objRow, objCol)) targetIsShaded = true;

        if ((obj.shadeRequirement == KickstartDataStructures.ShadeRequirement.NeedsShade && targetIsShaded == false) ||
            (obj.shadeRequirement == KickstartDataStructures.ShadeRequirement.NoShade && targetIsShaded == true))
        {
            Debug.Log("Shade requirement unmet");
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
                return false;
            }

            if (obj.needsAdjacentGround != KickstartDataStructures.GroundType.none && obj.needsAdjacentGround != groundType)
            {
                Debug.Log("Needed adjacent ground type missing");
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
                return false;
            }
        }

            return true;
    }
}
