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
    public LevelLayoutData levelLayoutData;
    public UIController uiController;
    
    public bool InDeleteMode { get; private set; }
    
    public void ToggleDeleteMode() => InDeleteMode = !InDeleteMode;
    public void SetDeleteMode(bool deleteMode) => InDeleteMode = deleteMode;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}
