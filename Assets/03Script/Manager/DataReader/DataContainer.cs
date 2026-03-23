using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Container", menuName = "DataReader/Container")]
public class DataContainer : ScriptableObject
{
    public URLReader reader;
    [SerializeField] byte mainLine = 5;

    [Tooltip("해당 시트 주소를 전체를 복사해서 넣어주세요.")]
    public string URL;

    [Tooltip("해당 시트의 실제 값 형태로 저장된 행의 갯수만큼 넣어주세요.")]
    public ScriptableObject[] objs;

    [HideInInspector] public bool isDataLoaded { get; private set; } = false;

    public void SetDatas(char splitSymbol, string[] lines)
    {
        isDataLoaded = false;

        if (lines == null)
        {
            Debug.LogError("Data 입력 없음.");
            return;
        }

        // 줄 단위로 들어온 데이터 가공
        for (int i = mainLine; i < lines.Length; i++)
        {
            if (i >= objs.Length + mainLine)
            {
                Debug.LogWarning($"<color=yellow>경고, {this.name}에 저장된 SO가 입력된 값보다 적습니다. SO를 추가하시거나 데이터 테이블을 점검해주세요.</color>");
                return;
            }
            string[] cols = lines[i].Split(splitSymbol);

            if (objs[i - mainLine] is IDataSeter )
            {
                (objs[i - mainLine] as IDataSeter).SetData(cols);
            }
            else
            {
                Debug.LogWarning($"{objs[i - 5].name}에 <color = red>IDataSeter</color>가 포함되어 있지 않습니다.");
            }
        }

        isDataLoaded = true;
    }
}
