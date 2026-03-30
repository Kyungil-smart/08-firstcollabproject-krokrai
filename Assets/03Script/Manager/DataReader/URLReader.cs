using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public struct  URLReader
{
    public char splitSymbol => '\t';

    public IEnumerator Load(string url, Action<char, string[]> SuccessCallback) //, Action FailureCallback) 실패 했을 때 게임에 대한 예외처리 사항을 등록해야 할때.
    {
        string sheetId = url.Split("d/")[1].Split('/')[0];
        string gid = url.Split("gid=")[1].Split('&')[0].Split('#')[0];

        string exprotUrl = $"https://docs.google.com/spreadsheets/d/{sheetId}/export?format=tsv&gid={gid}";

        using (UnityWebRequest uwr = UnityWebRequest.Get(exprotUrl))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"링크를 통해 데이터 읽어오기 실패. 에러 : {uwr.error}");
                yield break;
            }

            string sheetDataText = uwr.downloadHandler.text;
            string[] lines = sheetDataText.Split('\n');

            SuccessCallback?.Invoke(splitSymbol, lines);
        }
    }
}
