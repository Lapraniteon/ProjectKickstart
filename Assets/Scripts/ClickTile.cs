using System;
using UnityEngine;

public class ClickTile : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("RaycastTarget")))
            {
                Debug.Log(hit.point);
                int clickedTileX = (int)Mathf.Floor(hit.point.x);
                int clickedTileY = (int)Mathf.Floor(Mathf.Abs(hit.point.z));
                Vector2Int clickedTileCoordSpace = new Vector2Int(clickedTileY, clickedTileX);
                Debug.Log($"tile is in position -{clickedTileCoordSpace.x},{clickedTileCoordSpace.y}");

                // call the PlaceObject method in RuntimeBuildScript

                if (GameManager.Instance.InDeleteMode)
                {
                    GameManager.Instance.runtimeBuildScript.DeleteObject(clickedTileCoordSpace);
                }
                else
                {
                    GameManager.Instance.runtimeBuildScript.PlaceObject(clickedTileCoordSpace);
                }
                
            }
        }
    }
}
