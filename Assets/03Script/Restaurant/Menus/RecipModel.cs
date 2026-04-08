using UnityEngine;
using UnityEngine.UI;

public class RecipModel : MonoBehaviour
{
    //public int RecipID {  get; private set; }
    private RecipeContainer _recipe;
    [SerializeField] Button _btn;
    [SerializeField] GameObject Outline; // 선택 될시에만 선택.
    [SerializeField] Image _btnImage; // 언락 될 시 색을 (0,0,0) -> (255,255,255) 교체
    [SerializeField] Image _LockImage; // 언락 가능 시 깨진 자물쇠로 교체
    [SerializeField] Sprite _unlockSprite;
    [SerializeField] GameObject _ShildeImage; // 언락시 사라짐
    [SerializeField] private RecipeInfoUI _riu;
    [SerializeField] AddressableImageLoader _imgLoader;

    bool _canMake;
    bool _isUnlocked;
    bool _canUnlock;
    bool _isInitiated;

    private void Awake()
    {
        _canUnlock = false;
        _canMake = false;
        _isUnlocked = false;
        _isInitiated = false;
    }

    public void PassToRecipeInfo()
    {
        _riu.SelectedRecipe(_recipe, Outline, _canMake, _isUnlocked, _canUnlock);
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

        _imgLoader.SetImage(_recipe.dish_Sprite);

        _btn.onClick.AddListener(PassToRecipeInfo);
        // 추가적인 의존성 관련 필요한 경우 추가.
    }

    /// <summary>
    /// 특정 물고기를 잡아 레시피 해금 가능할 경우 호출
    /// </summary>
    public void CanUnlockConditionsMet()
    {
        Debug.Log($"{gameObject.name} 레시피 해금 됌");
        _canUnlock = true;
        _LockImage.sprite = _unlockSprite;
    }

    public void UnlockThisRecipe()
    {
        _isUnlocked = true;
        _canMake = true;
        _ShildeImage.SetActive(false);
        _btnImage.color = Color.white;
    }
}
