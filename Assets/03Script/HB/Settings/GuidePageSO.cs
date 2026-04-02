using UnityEngine;

[CreateAssetMenu(fileName = "GuidePage_", menuName = "SO/GuidePage")]
public class GuidePageSO : ScriptableObject, IDataSeter
{
    [Header("해당 페이지 이미지")]
    public Sprite guideImage;

    public void SetData(string[] datas)
    {
        
    }
}
