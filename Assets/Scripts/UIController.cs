using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject deleteModePanel;

    public Image cantPlaceIndicator;
    private TMP_Text cantPlaceIndicatorText;
    private string cantPlaceIndicatorTextStartingData;
    public Sequence cantPlaceIndicatorTween;

    public Image currentButtonSelected;

    public TMP_Text plantInformationHeader;
    public TMP_Text plantInformationText;
    
    public void ToggleDeleteModePanel() => deleteModePanel.SetActive(!deleteModePanel.activeSelf);
    public void SetDeleteModePanel(bool active) => deleteModePanel.SetActive(active);

    void Start()
    {
        cantPlaceIndicatorText = cantPlaceIndicator.GetComponentInChildren<TMP_Text>();
        cantPlaceIndicatorTextStartingData = cantPlaceIndicatorText.text;
    }
    
    public void FlashCantPlaceIndicator(string feedback)
    {
        cantPlaceIndicatorTween?.Kill();
        
        cantPlaceIndicatorText.text = $"{cantPlaceIndicatorTextStartingData}<br><size=50%>{feedback}</size>";
        cantPlaceIndicator.gameObject.SetActive(true);

        cantPlaceIndicatorTween = DOTween.Sequence()
            .Append(cantPlaceIndicator.DOFade(0.6f, 0.1f))
            .Join(cantPlaceIndicatorText.DOFade(1f, 0.1f))
            .AppendInterval(1f)
            .Append(cantPlaceIndicator.DOFade(0f, 0.5f))
            .Join(cantPlaceIndicatorText.DOFade(0f, 0.5f));
        
        cantPlaceIndicatorTween.Play().onComplete = () =>
        {
            cantPlaceIndicator.gameObject.SetActive(false);
        };
    }

    public void HideCantPlaceIndicator()
    {
        cantPlaceIndicatorTween?.Kill();

        cantPlaceIndicator.gameObject.SetActive(false);
    }

    public void SetSelectedButton(Image button)
    {
        if (currentButtonSelected != null)
            currentButtonSelected.color = new Color(1f, 1f, 1f, 1f);
        
        currentButtonSelected = button;
        
        if (currentButtonSelected != null && GameManager.Instance.runtimeBuildScript.selectedPrefabIndex != 0)
            currentButtonSelected.color = new Color(1f, 1f, 0.8f, 1f);
    }

    public void FinishLevel()
    {
        if (GameManager.Instance.levelRequirements.hardRequirementsComplete)
        {
            Debug.Log("Hard requirements complete, level can be finished");
        }
        else
        {
            Debug.Log("Level cannot be finished, hard requirements unmet");
        }
    }

    public void ClearPlantInfo()
    {
        plantInformationHeader.text = "";
        plantInformationText.text = "";
    }

    public void SetPlantInfo(int index)
    {
        ObjectTile obj = GameManager.Instance.runtimeBuildScript.GetPrefab(index);

        if (obj == null)
            return;
        
        plantInformationHeader.text = obj.formattedPlantHeader;
        plantInformationText.text = obj.formattedPlantInfo;
    }
    
}
