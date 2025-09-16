using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildLevel : MonoBehaviour
{

    public List<ObjectToSpawn> objectsToSpawn = new();

    public void BuildLevelObjects()
    {
        foreach (ObjectToSpawn obj in objectsToSpawn)
        {
            ObjectTile newObject = Instantiate(obj.prefab, new Vector3(obj.coords.x+0.5f, 0f, -obj.coords.y-0.5f), Quaternion.identity);

            if (obj.height == 1 && obj.width == 1)
            {
                GameManager.Instance.gameGrid.AddToObjectArray(newObject, obj.coords);
                continue;
            }

            for (int row = obj.coords.x; row < obj.coords.y + obj.height; row++)
            {
                for (int col = obj.coords.y; col < obj.coords.x + obj.width; col++)
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
