using UnityEngine;
using UnityEngine.UI;

public class MenuPanelController : MonoBehaviour
{
    // TODO : 임시로 열고 사용 fish, recipe 모두 사용 후 참조 받는 걸로 변환 필요. @@@@@@
    [SerializeField] RecipeContainer _recipe;
    [SerializeField] RecipeInfoUI _recipeInfoUI;
    [SerializeField] MenuCtrl _menuCtrl;

    RecipModel _currentRecipe;

    bool isUnlocked;

    private void Awake()
    {
        isUnlocked = false;
    }

    private void OnDisable()
    {
        _recipe = null;
    }

    // 레시피 선택 되면 어떤 레시피인지 메뉴 판에 넣기.
    // 테스트용 임시 주석 처리
    public void SelectedRecipe() //(RecipeContainer rcp)
    {
        //_currentRecipe.
        //_recipe = rcp;
        //_recipeInfoUI.SelectedRecipe(_recipe, canMake , isUnlocked);

        //CanMakeDish();
    }
    
    // 선택된 레시피가 요리하기를 눌렀다면, 해당 함수에서 메뉴 crtl에 넣겨주기 현재 대기열 max인 경우 요리하기 못하게 하기..
    public void MakeDish() // (RecipeContainer rcp)
    {
        // 현재 열려 있는 메뉴 칸 보다 더 넣을려고 할때 예외처리. 버튼 자체가 작동 안하게 해야됌
        _menuCtrl.InsertDish(_recipe);
    }
}
