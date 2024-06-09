using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameMainFlow : MonoBehaviour
{
    void Awake()
    {
        Global.Instance.Init();
        TCPController.Instance.Init();
    }
}
