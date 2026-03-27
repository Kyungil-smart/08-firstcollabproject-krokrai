using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FishSlot : MonoBehaviour
{
    [SerializeField] private FishData _fishData;
    public FishData fishData => _fishData;
    public Image fishImage;     // 물고기 이미지

    private FishListManager _fishListManager;

    public void Setup(FishData fishData, FishListManager fishListManager)
    {
        _fishData = fishData;
        _fishListManager = fishListManager;

        if (fishImage != null)
        {
            if (fishData.isCaught)
            {
                fishImage.sprite = fishData.fishSprite;
            }
            
            else
            {
                fishImage.sprite = fishData.silhouetteSprite;
            }
        }

        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnSlotClicked);
        }
    }

    private void OnSlotClicked()
    {
        if (_fishListManager != null)
        {
            _fishListManager.ChangeFishData(_fishData);
        }
    }
}
