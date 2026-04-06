using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeInfoUI : MonoBehaviour
{
    [SerializeField] Image[] _img; // 1 : 요리될 메뉴 사진, 2번 금액, 3번 생산수, 4번 물고기
    [SerializeField] GameObject[] _panels; // 1 : 요리, 2. 해금
    [SerializeField] TextMeshProUGUI[] _tmpUGUI; // 1 : 메뉴 이름 2: 판매액, 3번 생산수, 4번 음식 설명, 5번 물고기 이름, 6번 물고기 소지 / 소모
    [SerializeField] Image[] _UnlockImg;
    [SerializeField] TextMeshProUGUI[] _tmpUGUIUnlock; // 1 : 생선 이름, 2: 획득 여부
    [SerializeField] Button _btn;
    [SerializeField] MenuCtrl _menuctrl;
    [SerializeField] InventorySystem _inventorySystem;

    RecipeContainer _rcp;

    FishData fish;
    Language _lng; // 현재 언어

    bool _canMake;
    byte _currentHasFish;

    public void ThisRecipeCanMake(bool b) => _canMake = !b;

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
    public void SelectedRecipe(in RecipeContainer rcp, in bool canMake, in bool isUnlcok)
    {
        if ( canMake && _canMake )


        // 렌더러 작업은 스프라이터 관련 매니저 완성 후
        /*
        _img[0].sprite =  여기에 나중에 만들 스프라이트 접근용 함수 입력
         */

        _rcp = rcp;

        if (DataTower.instance.fishDatas.ContainsKey(rcp.ingredient))
            fish = DataTower.instance.fishDatas[rcp.ingredient];
        else


            // TMP 관련
        _tmpUGUI[1].text = (rcp.prices 
                * ( 1 + DataTower.instance.BonusDishPrice01Level + DataTower.instance.BonusDishPrice02Level) )
                .ToString();
        _tmpUGUI[2].text = rcp.yield.ToString();
        

        if( !isUnlcok )
        {
            _panels[0].SetActive(true);
            _panels[1].SetActive(false);

            for (byte i = 0; i < DataTower.instance.Items.Count; i++)
            {
                if (fish.fishID == DataTower.instance.Items[i].fishID)
                    _currentHasFish++;
            }

            _tmpUGUI[5].text = $"{_currentHasFish} / 1";
            switch (_lng)
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
        }
        else
        {
            _panels[1].SetActive(true);
            _panels[0].SetActive(false);
            if (fish.isCaught)
            {

            }
            else
            {
                switch (_lng)
                {
                    case Language.KOR:

                        break;
                    case Language.ENG:

                        break;
                }
            }
        }
            

        

        // TODO : 물고기 소지 및 관련 인벤토리 작업 완료 후 진입. @@@@@@@@@
    }
    public void MakeMenu()
    {
        _currentHasFish--;
        _inventorySystem.Erase(fish.fishID);
        _menuctrl.InsertDish(_rcp);
    }
}
