using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameSingleton : MonoBehaviour
{
    public void Start()
    {
        PlayerController.Instance.Init();
        InGameSessionController.Instance.Init();
    }
}
