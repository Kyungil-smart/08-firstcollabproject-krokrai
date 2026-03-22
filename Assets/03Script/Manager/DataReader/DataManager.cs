using System;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] URLReader reader;

    [SerializeField] private DataContainer[] container;

    private void Start()
    {
        for (int i = 0; i < container.Length; i++)
        {
            StartCoroutine(reader.Load(container[i].URL, container[i].SetDatas));
        }
    }
}
