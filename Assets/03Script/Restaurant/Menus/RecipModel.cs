using UnityEngine;

public class RecipModel : MonoBehaviour
{
    //public int RecipID {  get; private set; }
    private RecipeContainer _recipe;

    bool _canMake;
    bool _isUnlocked;
    bool _canUnlock;
    bool _isInitiated;
    bool _isSelected;


    private void Awake()
    {
        _canUnlock = false;
        _canMake = false;
        _isUnlocked = false;
        _isInitiated = false;
        _isSelected = false;
    }

    public void InitRecip(RecipeContainer rcp)
    {
        if (_isInitiated)
        {
            Debug.Log("레피시에 대한 추가적인 초기화가 시도가 있습니다.");
            return;
        }

        _isInitiated = true;
        _recipe = rcp;
        // 추가적인 의존성 관련 필요한 경우 추가.
    }

    /// <summary>
    /// 특정 물고기를 잡아 레시피 해금 가능할 경우 호출
    /// </summary>
    public void UnlockConditionsMet()
    {
        // Action으로 특정 물고기 숫자가 1 이상인 경우에만 호출 되게 설계 예정.
        _canUnlock = true;
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
