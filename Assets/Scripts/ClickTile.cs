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

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.point);
                int clickedTileX = (int)Mathf.Floor(hit.point.x);
                int clickedTileY = (int)Mathf.Ceil(Mathf.Abs(hit.point.z));
                Debug.Log($"tile is in position -{clickedTileY},{clickedTileX}");

                // call the PlaceObject method in RuntimeBuildScript

                GameManager.Instance.runtimeBuildScript.PlaceObject(new Vector2Int(clickedTileY, clickedTileX));
            }
        }
    }
}
