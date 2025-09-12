using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance; // Game Manager singleton pattern
    public static GameManager Instance
    {
        get
        {
            if (_instance is null) // Error checking in case the Game Manager is not assigned
                Debug.LogError("GameManager is null!");

            return _instance;
        }
    } // Game Manager instance property

    private void Awake()
    {
        _instance = this;
    }



    public GameGrid gameGrid;
    
    public RuntimeBuildScript runtimeBuildScript;
    [HideInInspector] public LevelRequirements levelRequirements;
}
