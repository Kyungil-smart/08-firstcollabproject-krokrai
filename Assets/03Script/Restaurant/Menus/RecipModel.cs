using UnityEngine;
using UnityEngine.UI;

public class RecipModel : MonoBehaviour
{
    //public int RecipID {  get; private set; }
    private RecipeContainer _recipe;
    [SerializeField] Button _btn;
    private RecipeInfoUI _riu;

    bool _canMake;
    bool _isUnlocked;
    bool _canUnlock;
    bool _isInitiated;

    private void Start()
    {
        _canUnlock = false;
        _canMake = false;
        _isUnlocked = false;
        _isInitiated = false;
    }

    private void PassToRecipeInfo()
    {
        //_riu.SelectedRecipe(_recipe, _canMake);
        _riu.SelectedRecipe(_recipe, true);
    }

    private void OnDestroy()
    {
        _btn.onClick.RemoveAllListeners();
    }

    public void InitRecip(RecipeContainer rcp, RecipeInfoUI riu)
    {
        if (_isInitiated)
        {
            Debug.Log("레피시에 대한 추가적인 초기화가 시도가 있습니다.");
            return;
        }

        _isInitiated = true;
        _riu = riu;
        _recipe = rcp;

        _btn.onClick.AddListener(PassToRecipeInfo);
        // 추가적인 의존성 관련 필요한 경우 추가.
    }

    /// <summary>
    /// 특정 물고기를 잡아 레시피 해금 가능할 경우 호출
    /// </summary>
    public void CanUnlockConditionsMet()
    {
        _canUnlock = true;
    }

    public void UnlockThisRecipe()
    {
        _canMake = true;
    }

    /// <summary>
    /// 해당 물고기가 있어 만들 수 있는 지 판단.
    /// </summary>
    /// <param name="canUnlock"></param>
    public void CanMakeDish(bool canMake)
    {
        // Action으로 특정 물고기 숫자가 1 이상인 경우에만 호출 되게 설계 예정.
        _canMake = canMake;
    }

}
