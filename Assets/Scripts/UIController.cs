using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject deleteModePanel;
    
    public void ToggleDeleteModePanel() => deleteModePanel.SetActive(!deleteModePanel.activeSelf);
    public void SetDeleteModePanel(bool active) => deleteModePanel.SetActive(active);
}
