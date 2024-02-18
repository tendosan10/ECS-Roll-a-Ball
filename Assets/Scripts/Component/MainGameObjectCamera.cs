using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MainCameraオブジェクトのインスタンスを保持する
/// </summary>
public class MainGameObjectCamera : MonoBehaviour
{
    public static Camera Instance;

    void Awake()
    {
        Instance = GetComponent<Camera>();
    }
}