using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchingController : MonoBehaviour
{
    private static MatchingController instance;
    public static MatchingController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MatchingController>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("MatchingControllerSingleton");
                    instance = singletonObject.AddComponent<MatchingController>();
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return instance;
        }
    }
    public void Init()
    {
        Debug.Log($"{this.ToString()} Init Complete");
    }

    public void ProcessMatchingPacket(byte[] realData, int length)
    {
        if ((MatchProtocol)realData[0] == MatchProtocol.GameMatched)
        {
            TCPController.Instance.EnqueueDispatcher(() =>
            {
                Action okaction = () =>
                {
                    Packet packet = new Packet();

                    int length = 0x01 + 0x01 + Utils.GetLength(Global.Instance.standbyInfo.userEntity.UserUID);

                    packet.push((byte)Protocol.Match);
                    packet.push(length);
                    packet.push((byte)MatchProtocol.GameAccept);
                    packet.push(Global.Instance.standbyInfo.userEntity.UserUID);

                    TCPController.Instance.SendToServer(packet);

                    SceneManager.LoadScene("InGame");
                };
                Action Cancelaction = () =>
                {
                    Global.Instance.standbyInfo.gameType = GameType.Default;

                    Global.Instance.standbyInfo.isMatchingNow = false;

                    MainView mainView = FindObjectOfType<MainView>(true);

                    mainView.ChangeGameQueueStatus();
                };
                PopupController.Instance.SetActivePopupWithMessage(POPUPTYPE.OKCANCEL, true, 6, okaction, Cancelaction);
            });
        }
    }
}
