using System.Linq;
using TMPro;
using UnityEngine;

public class LevelRequirements : MonoBehaviour
{

    public LevelRequirement[] hardRequirements;
    public LevelRequirement[] softRequirements;
    
    public bool hardRequirementsComplete { get; private set; }

    private void Awake()
    {
        
    }
    
    private void Start()
    {
        GameManager.Instance.levelRequirements = this;
        SetTextObjects();
    }

    private void SetTextObjects()
    {
        foreach (LevelRequirement levelRequirement in hardRequirements.Concat(softRequirements))
        {
            levelRequirement.textObject.text = levelRequirement.completed ? $"<s>{levelRequirement.description}</s>" : levelRequirement.description;
        }
    }
    
    public void Check()
    {
        GameGrid gameGrid = GameManager.Instance.gameGrid;

        // Check hard requirements

        // Plant 25 yellow plants.
        hardRequirements[0].completed = gameGrid.CountObjectsWithTag(KickstartDataStructures.Color.Yellow,
            KickstartDataStructures.ObjectType.Plant) >= 25;
        Debug.Log($"{hardRequirements[0].completed}: {hardRequirements[0].description}");

        // Plant at least 5 waterlilies.
        hardRequirements[1].completed =
            gameGrid.CountObjectsWithTag("Lily", KickstartDataStructures.ObjectType.Plant) >= 5;
        Debug.Log($"{hardRequirements[1].completed}: {hardRequirements[1].description}");

        // Waterlilies may not be placed next to each other.
        hardRequirements[2].completed = gameGrid.AreThereAdjacentObjects("Lily", "Lily");
        Debug.Log($"{hardRequirements[2].completed}: {hardRequirements[2].description}");

        // Yellow plants may not be next duck statues.
        hardRequirements[3].completed =
            gameGrid.AreThereAdjacentObjects("DuckStatue", null, KickstartDataStructures.Color.Yellow);
        Debug.Log($"{hardRequirements[3].completed}: {hardRequirements[3].description}");

        hardRequirementsComplete = true;
        foreach (LevelRequirement hardRequirement in hardRequirements)
        {
            if (!hardRequirement.completed)
            {
                hardRequirementsComplete = false;
                break;
            }
        }

        // Check soft requirements

        // Plant at least 4 different plant colors.
        softRequirements[0].completed = gameGrid.CountAmountOfPlantColors() >= 4;
        Debug.Log($"{softRequirements[0].completed}: {softRequirements[0].description}");

        // Place 6 duck statues.
        softRequirements[1].completed =
            gameGrid.CountObjectsWithTag("DuckStatue", KickstartDataStructures.ObjectType.Decoration) >= 6;
        Debug.Log($"{softRequirements[1].completed}: {softRequirements[1].description}");

        // Occupy every soil space.
        softRequirements[2].completed = IsEverySoilSpaceOccupied(gameGrid);
        Debug.Log($"{softRequirements[2].completed}: {softRequirements[2].description}");
        
        SetTextObjects();
    }

    private static bool IsEverySoilSpaceOccupied(GameGrid gameGrid)
    {
        for (int row = 1; row < gameGrid.height - 1; row++)
        {
            for (int col = 1; col < gameGrid.width - 1; col++)
            {
                if (gameGrid.groundArray[row, col] == null) // Skip if there is nothing on the ground array
                    continue;

                if (gameGrid.groundArray[row, col].type !=
                    KickstartDataStructures.GroundType.Soil) // Skip if the tile is not soil
                    continue;

                if (gameGrid.groundArray[row, col].placeable == false) // Skip if the tile cannot be placed on
                    continue;

                if (gameGrid.objectArray[row, col] == null) // No object present on a space where there should be one
                {
                    return false;
                }
            }
        }

        return true;
    }
}

[System.Serializable]
public class LevelRequirement
{
    public string description;

    public bool completed = false;

    public TMP_Text textObject;
}
