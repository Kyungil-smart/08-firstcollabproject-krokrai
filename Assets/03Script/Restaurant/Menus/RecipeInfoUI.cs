using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeInfoUI : MonoBehaviour
{
    [SerializeField] Image[] _img; // 1 : 요리될 메뉴 사진, 2번 금액, 3번 생산수, 4번 물고기
    [SerializeField] TextMeshProUGUI[] _tmpUGUI; // 1 : 메뉴 이름 2: 판매액, 3번 생산수, 4번 음식 설명, 5번 물고기 이름, 6번 물고기 소지 / 소모
    [SerializeField] Button _btn;
    [SerializeField] MenuCtrl _menuctrl;

    FishData fish;
    Language _lng; // 현재 언어

    bool _canMake;

    public void ThisRecipeCanMake(bool b) => _canMake = b;

    public void ChangedLanguage(Language lng)
    {
        _lng = lng;
    }

    private void OnEnable()
    {
        _menuctrl.OnMaxMenu += ThisRecipeCanMake;
    }

    private void OnDisable()
    {
        _menuctrl.OnMaxMenu -= ThisRecipeCanMake;
    }

    /// <summary>
    /// 선택된 레시피에 대한 정보 띄우기
    /// </summary>
    public void SelectedRecipe(in RecipeContainer rcp, in bool canMake)
    {
        if ( canMake && _canMake )
            _btn.interactable = true;
        else
            _btn.interactable = false;

        // 렌더러 작업은 스프라이터 관련 매니저 완성 후
        /*
        _img[0].sprite =  여기에 나중에 만들 스프라이트 접근용 함수 입력
         */

        if (DataTower.instance.fishDatas.ContainsKey(rcp.ingredient))
            fish = DataTower.instance.fishDatas[rcp.ingredient];
        else


            // TMP 관련
            _tmpUGUI[1].text = rcp.prices.ToString();
        _tmpUGUI[2].text = rcp.yield.ToString();
        switch(_lng)
        {
            case Language.KOR:
                _tmpUGUI[0].text = rcp.recipe_KName;
                _tmpUGUI[3].text = rcp.KDescription.ToString();
                _tmpUGUI[4].text = fish.korName.ToString();
                break;
            case Language.ENG:
                _tmpUGUI[0].text = rcp.recipe_EName;
                _tmpUGUI[3].text = rcp.EDescription.ToString();
                _tmpUGUI[4].text = fish.korName.ToString();
                break;
        }

        // TODO : 물고기 소지 및 관련 인벤토리 작업 완료 후 진입. @@@@@@@@@
    }
}
