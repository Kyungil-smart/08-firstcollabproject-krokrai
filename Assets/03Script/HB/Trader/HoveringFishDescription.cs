using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoveringFishDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject _descriptionPanel;       // 띄워줄 창
    private TextMeshProUGUI _nameText;          // 이름 텍스트
    private TextMeshProUGUI _rarityText;        // 등급 텍스트

    private Coroutine _routine;                 // 대기 중인 코루틴 제어
    private FishData _fishData;                 // 표시할 물고기 데이터

    public void Setup(FishData fishData, GameObject panel, TextMeshProUGUI nameText, TextMeshProUGUI rarityText)
    {
        _fishData = fishData;
        _descriptionPanel = panel;
        _nameText = nameText;
        _rarityText = rarityText;

        if (DataTower.instance != null)
        {
            DataTower.instance.OnLanguageSettingChanged -= RefreshText;
            DataTower.instance.OnLanguageSettingChanged += RefreshText;
        }
    }

    private void OnDestroy()
    {
        if(DataTower.instance != null)
        {
            DataTower.instance.OnLanguageSettingChanged -= RefreshText;
        }
    }

    // 마우스가 FishSlot에 들어갔을 때 호출
    public void OnPointerEnter (PointerEventData eventData)
    {   
        // 물고기 데이터나 패널이 없으면 실행x
        if (_fishData == null || _descriptionPanel == null) return;

        _routine = StartCoroutine(ShowDescriptionPanel());
    }

    public void OnPointerExit (PointerEventData eventData)
    {
        // 대기 중이라면 코루틴 중단
        if (_routine != null) StopCoroutine(_routine);

        // 설명창 끔
        if (_descriptionPanel != null) _descriptionPanel.SetActive(false);
    }


    private IEnumerator ShowDescriptionPanel()
    {
        // 0.5초 동안 대기
        yield return new WaitForSeconds(0.5f);

        RefreshText(DataTower.instance.languageSetting);

        RectTransform rect = GetComponent<RectTransform>();

        Vector3[] corners = new Vector3[4];

        rect.GetWorldCorners(corners);

        _descriptionPanel.transform.position = corners[2] + new Vector3(10f, 0, 0);

        _descriptionPanel.SetActive(true); 
    }

    private void RefreshText(Language language)
    {
        if(_fishData == null || _nameText == null || _rarityText == null) return;

        // 한글인지 영문인지
        _nameText.text = (language == Language.KOR) ? _fishData.korName : _fishData.engName;

        // 머릿말
        string rarityHeader = (language == Language.KOR) ? "등급" : "Rarity";

        // 등급: 내용 한영스왑
        _rarityText.text = $"{rarityHeader} : [{_fishData.fishRarity.ToName()}]";
    }
}
