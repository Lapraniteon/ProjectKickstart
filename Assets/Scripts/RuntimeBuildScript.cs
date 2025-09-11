using UnityEngine;

public class RuntimeBuildScript : MonoBehaviour
{
    public ObjectTile[] PrefabList;
    public int selectedPrefabIndex = 0; // should move this to the game controller probably

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
            if (!RequirementCheck())
            {
                Destroy(newObject);
            } else
            {
                GameManager.Instance.gameGrid.Add(newObject, coordinates);
            }
        }


    }

    bool RequirementCheck()
    {
        return true;

        // check if spot is empty
        //check for correct type of soil
        // check shade requirement -> 1 for ground, 1 for shade providing plants. ground OR plant is true -> shade. 
        // check soil adjacency requirements
        // check plant adjacency requirements
    }
}
