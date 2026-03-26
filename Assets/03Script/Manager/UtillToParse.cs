using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public static class UtillToParse
{
    /// <summary>
    /// out에 넣은 변수에 값을 반환합니다.
    /// </summary>
    /// <param name="s"></param>
    /// <param name="i">반환된 값을 받을 변수</param>
    /// <param name="objname">오류시 추적용 값</param>
    public static void TryParseForSO(this string s, out int i, in string objname = "지정된 경로 없음")
    {
        if (s == null || !int.TryParse(s, out i))
        {
            Debug.LogWarning($"{objname}에 들러온 문자열이 유효하지 않은 값입니다. -1을 반환합니다.");
            i = -1;
        }
    }

    /// <summary>
    /// out에 넣은 변수에 값을 반환합니다.
    /// </summary>
    /// <param name="s"></param>
    /// <param name="i">반환된 값을 받을 변수</param>
    /// <param name="objname">오류시 추적용 값</param>
    public static void TryParseForSO(this string s, out float i, in string objname = "지정된 경로 없음")
    {
        if (s == null || !float.TryParse(s, out i))
        {
            Debug.LogWarning($"{objname}에 들러온 문자열이 유효하지 않은 값입니다. -1을 반환합니다.");
            i = -1;
        }
    }

    public static void TryParseForSO(this string s, out bool i, in string objname = "지정된 경로 없음")
    {
        if (s != null)
        {
            switch(s)
            {
                case "FALSE":
                    i = false;
                    return;
                case "TRUE":
                    i = true;
                    return;
                default:
                    Debug.LogWarning($"{objname}에 들러온 문자열이 유효하지 않은 값입니다. false를 반환합니다.");
                    i = false;
                    return;
            }
        }
        else
        {
            Debug.LogWarning($"{objname}에 들러온 문자열이 유효하지 않은 값입니다. false를 반환합니다.");
            i = false;
        }
    }
}
