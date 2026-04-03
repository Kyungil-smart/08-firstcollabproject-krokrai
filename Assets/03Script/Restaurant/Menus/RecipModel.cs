using UnityEngine;
using UnityEngine.UI;

public class RecipModel : MonoBehaviour
{
    //public int RecipID {  get; private set; }
    private RecipeContainer _recipe;
    [SerializeField] Button _btn;
    [SerializeField] GameObject Outline; // 선택 될시(아마도 언락인 경우만)에만 선택.
    [SerializeField] Image _btnImage; // 언락 될 시 색을 (0,0,0) -> (255,255,255) 교체
    [SerializeField] Image _LockImage; // 언락 가능 시 깨진 자물쇠로 교체
    [SerializeField] GameObject _ShildeImage; // 언락시 사라짐
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
        _riu.SelectedRecipe(_recipe, _canMake, _canUnlock);
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

        //_btnSprtRenderer.sprite =  // 스프라이트 추가 명령

        _btn.onClick.AddListener(PassToRecipeInfo);
        // 추가적인 의존성 관련 필요한 경우 추가.
    }

    /// <summary>
    /// 특정 물고기를 잡아 레시피 해금 가능할 경우 호출
    /// </summary>
    public void CanUnlockConditionsMet()
    {
        _canUnlock = true;
        //_LockImage.sprite = ; // 깨진 자물쇠 받아와서 적용
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
        // 잡은 물고기 음식 만들때 상위에서 호출 후 여기로 넘어오게 하면 될듯
        _canMake = canMake;
        if (_canMake)
        {
            // 물고기가 있는 경우. 적용
        }
        else
        {
            // 물고기가 없는 경우 적용.
        }

    }

}
