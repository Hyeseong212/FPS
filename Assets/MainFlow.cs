using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainFlow : MonoBehaviour
{
    private void Awake()
    {
        TCPController.Instance.StartTCPController();
    }
}
