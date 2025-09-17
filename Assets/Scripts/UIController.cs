using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject deleteModePanel;

    public Image cantPlaceIndicator;
    private TMP_Text cantPlaceIndicatorText;
    public Sequence cantPlaceIndicatorTween;
    
    public void ToggleDeleteModePanel() => deleteModePanel.SetActive(!deleteModePanel.activeSelf);
    public void SetDeleteModePanel(bool active) => deleteModePanel.SetActive(active);

    void Start()
    {
        cantPlaceIndicatorText = cantPlaceIndicator.GetComponentInChildren<TMP_Text>();
    }
    
    public void FlashCantPlaceIndicator()
    {
        cantPlaceIndicatorTween?.Kill();
        
        cantPlaceIndicator.gameObject.SetActive(true);

        cantPlaceIndicatorTween = DOTween.Sequence()
            .Append(cantPlaceIndicator.DOFade(1f, 0.1f))
            .Join(cantPlaceIndicatorText.DOFade(1f, 0.1f))
            .AppendInterval(0.4f)
            .Append(cantPlaceIndicator.DOFade(0f, 0.5f))
            .Join(cantPlaceIndicatorText.DOFade(0f, 0.5f));
        
        cantPlaceIndicatorTween.Play().onComplete = () =>
        {
            cantPlaceIndicator.gameObject.SetActive(false);
        };
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
    
}
