using UnityEngine;

public class ObjectTile : MonoBehaviour
{
    [Tooltip("Can this object be moved or removed?")]
    public bool canBeEdited = true;

    [Tooltip("What type of object is this?")]
    public KickstartDataStructures.ObjectType objectType;
}
