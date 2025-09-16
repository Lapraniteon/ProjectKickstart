using System.Collections.Generic;
using UnityEditor;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class BuildLevel : MonoBehaviour
{

    public List<ObjectToSpawn> objectsToSpawn = new();

    public void BuildLevelObjects()
    {
        foreach (ObjectToSpawn obj in objectsToSpawn)
        {
            Vector2Int newCoords = new Vector2Int(obj.coords.y, obj.coords.x);
            
            ObjectTile newObject = Instantiate(obj.prefab, new Vector3(newCoords.y + 0.5f, 0f, -newCoords.x - 0.5f), Quaternion.identity);

            if (obj.height == 1 && obj.width == 1)
            {
                GameManager.Instance.gameGrid.AddToObjectArray(newObject, newCoords);
                continue;
            }

            for (int row = newCoords.x; row < newCoords.x + obj.height; row++)
            {
                for (int col = newCoords.y; col < newCoords.y + obj.width; col++)
                {
                    GameManager.Instance.gameGrid.AddToObjectArray(newObject, new Vector2Int(row, col));
                }
            }
        }
    }

}

[System.Serializable]
public class ObjectToSpawn
{
    public ObjectTile prefab;
    public Vector2Int coords;
    public int height = 1;
    public int width = 1;
}
