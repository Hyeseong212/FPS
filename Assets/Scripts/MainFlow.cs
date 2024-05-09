using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainFlow : MonoBehaviour
{
    private void Awake()
    {
        Global.Instance.Init();
        PopupController.Instance.Init();
        TCPController.Instance.Init();
    }
}
