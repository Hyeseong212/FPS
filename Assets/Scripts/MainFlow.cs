using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainFlow : MonoBehaviour
{
    private void Awake()
    {
        Global.Instance.Init();
        ViewController.Instance.Init();
        GuildController.Instance.Init();
        LoginController.Instance.Init();
        ChatController.Instance.Init();
        PopupController.Instance.Init();
        MatchingController.Instance.Init();
        TCPController.Instance.Init();
    }
}
