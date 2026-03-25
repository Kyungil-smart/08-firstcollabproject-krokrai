/*
    <레시피 관리자>
물고기에 대하 고유 번호 필요 ( 물고기를 소모한 경우 전달 등을 해야되기 때문)
레시피에서 사용하는 물고기 번호를 여기에 넘겨주면 DataTower에서 검색
레시피에 대한 참조 필요 (물고기가 0이 되거나 1이 됬을때 상태를 전달해야되기 때문에.)

Action으로 만들 수 있는 지 없는지만, 넘겨주기.

overload를 통해 최대 4개까지 받고 넘겨주기.

 */
using System;
using UnityEngine;

public class RecipManager : MonoBehaviour
{
    DataTower _dataTower;
    RecipModel[] _recipModels; // 매니저가 SO에 대한 접근 및 지정 필요
    // 1개인 경우에는 딕셔너리 사용해도 되는데, 4개인 경우에는 어떻게 해금 할 것인지???

    Action<int> OnFishDataUpdate;

    private void AddRecipData()
    {
        for (int i = 0; i < _recipModels.Length; i++)
        {
            OnFishDataUpdate += _recipModels[i].UnlockConditionsMet;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < _recipModels.Length; i++)
        {
            OnFishDataUpdate -= _recipModels[i].UnlockConditionsMet;
        }
    }

    /// <summary>
    /// Data Tower에서 물고기 갯수를 받아오기 위한 함수.
    /// </summary>
    /// <param name="fishID"></param>
    /// <returns></returns> 
    public uint CheckHaveFish(uint fishID)
    {
        //return _dataTower.; // 해당 물고기 갯수를 타워에서 검색 
        return 10; // 임시 작업
    }

    /// <summary>
    /// 낚시에 성공한 물고기가 처음 낚인 물고기인 경우. n(임시)에 값을 기준으로 해금
    /// </summary>
    /// <param name="n"></param>
    public void FirstFishingFish(int n)
    {
        OnFishDataUpdate?.Invoke(n);
    }
    // Data Tower에서 3가지 Action을 만들어서 1번 : 들어오거나 나가는 경우, 2번 : 갯수가 0이 된경우, 3번 : 갯수가 0이 아닌경우. 4번 : 도감용 첫 획득인 경우.
}
