using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginController : MonoBehaviour
{
    private static LoginController instance;
    public static LoginController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LoginController>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("LoginControllerSingleton");
                    instance = singletonObject.AddComponent<LoginController>();
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return instance;
        }
    }
    public void Init()
    {
        Debug.Log("LoginController Init Complete");
    }
    public void ProcessLoginPacket(byte[] realData) 
    {
        if ((LoginRequestType)realData[0] == LoginRequestType.LoginRequest)
        {
            if ((ResponseType)realData[1] == ResponseType.Success)
            {
                long sendUseruidval = BitConverter.ToInt64(realData, 2);
                Global.Instance.standbyInfo.userUid = sendUseruidval;
                TCPController.Instance.EnqueueDispatcher(() =>
                {
                    Debug.Log("�α� ����");
                    ViewController.Instance.SetActiveView(VIEWTYPE.LOGIN, false);
                });
            }
            else if ((ResponseType)realData[1] == ResponseType.Fail)
            {
                Debug.Log("�� �α��� ���� ����");
            }
        }
        else if ((LoginRequestType)realData[0] == LoginRequestType.LogoutRequest)
        {
            if ((ResponseType)realData[1] == ResponseType.Success)
            {
                //Debug.Log(message);
                TCPController.Instance.EnqueueDispatcher(() =>
                {
                    ViewController.Instance.SetActiveView(VIEWTYPE.LOGIN, true);
                    Debug.Log("�α׾ƿ� ����");
                });
            }
            else if ((ResponseType)realData[1] == ResponseType.Fail)
            {
                Debug.Log("�� �α׾ƿ� ���� ����");
            }
        }
        else if ((LoginRequestType)realData[0] == LoginRequestType.SignupRequest)
        {
            if ((ResponseType)realData[1] == ResponseType.Success)
            {
                //Debug.Log(message);
                TCPController.Instance.EnqueueDispatcher(() =>
                {
                    ViewController.Instance.SetActiveView(VIEWTYPE.SIGNUP, false);
                    ViewController.Instance.SetActiveView(VIEWTYPE.LOGIN, true);
                    PopupController.Instance.SetActivePopupWithMessage(POPUPTYPE.MESSAGE, true, 1, null);
                    Debug.Log("ȸ������ ����");
                });
            }
            else if ((ResponseType)realData[1] == ResponseType.Fail)
            {
                TCPController.Instance.EnqueueDispatcher(() =>
                {
                    PopupController.Instance.SetActivePopupWithMessage(POPUPTYPE.MESSAGE, true, 0, null);
                    Debug.Log("�� ȸ������ ���� ����");
                });
            }
        }
    }
}
