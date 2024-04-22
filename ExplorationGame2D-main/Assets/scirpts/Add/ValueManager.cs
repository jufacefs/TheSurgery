using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueManager : MonoBehaviour
{
    public static ValueManager Instance;

    public int currentValue = 1;  // 设定游戏开始时的初始值为1

    private static bool isInitialized = false;

    void Awake()
    {
        if (!isInitialized)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            currentValue = 1;  // 游戏开始时设置为1
            isInitialized = true;
        }
    }

    public void IncrementValue()
    {
        currentValue++;  // 增加当前值
    }

    public void ResetValue()
    {
        currentValue = 1;  // 重置为1
    }
}
