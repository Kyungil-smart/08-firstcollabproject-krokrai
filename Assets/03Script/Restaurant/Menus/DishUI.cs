using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DishUI : MonoBehaviour
{
    [SerializeField] Image[] _img; // 1 : 메뉴 사진, 2 : 남은 수량
    [SerializeField] TextMeshProUGUI[] _tmpUGUI; // 1 : 메뉴 이름, 2 : 남은 수량

    // 임시로 전부 인스펙터에 노출 후 작업
    [SerializeField] RecipeContainer _rcp;

    // SO 그대로 받아서 사용할 시 금액, 생산량이 달라진 상태로 넘어 올 수 있는지 확인 필요.
    public void DishOnDish(RecipeContainer rcp)
    {
        _rcp = rcp;

        _tmpUGUI[0].text = _rcp.name;
        _tmpUGUI[1].text = _rcp.yield.ToString();
    }

    // 실제 메뉴가 정렬 방식 선택 필요,.

    public void EatMenu()
    {
        if (_rcp.yield < 0)
            DeletMenu();

        _rcp.yield--;
        _tmpUGUI[1].text = _rcp.yield.ToString();
    }

    public void DeletMenu()
    {
        _rcp = null;
        
        // image 초기화

        // tmp 초기화
        for (int i = 0; i < _tmpUGUI.Length; i++)
        {
            _tmpUGUI[i].text = "";
        }
    }

}
