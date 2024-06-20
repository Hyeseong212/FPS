using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InGameSessionController : MonoBehaviour
{
    //���⼭ ���������� ��ó���ع����� �Ӹ�����
    private static InGameSessionController instance;
    public static InGameSessionController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InGameSessionController>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("InGameSessionControllerSingleton");
                    instance = singletonObject.AddComponent<InGameSessionController>();
                    var singletonParent = FindObjectOfType<InGameSingleton>();
                    Instantiate(singletonObject, singletonParent.transform);
                }
            }
            return instance;
        }
    }

    public InGameSessionInfo inGameSessionInfo;

    public void Init()
    {
        Global.Instance.StaticLog($"{this.ToString()} Init Complete");
        inGameSessionInfo = new InGameSessionInfo();
    }

    public void ProcessSessionPacket(byte[] realData)
    {
        if((SessionInfo)realData[0] == SessionInfo.SessionSyncOK)
        {
            Global.Instance.StaticLog("SessionSyncOK Packet");

            Packet packet = new Packet();

            int length = 0x01 + Utils.GetLength(Global.Instance.standbyInfo.userEntity.UserUID);

            packet.push((byte)InGameProtocol.SessionInfo);
            packet.push(length);
            packet.push((byte)SessionInfo.PlayerNum);
            packet.push(Global.Instance.standbyInfo.userEntity.UserUID);
            //�ε� 1�ܰ�Ϸ� ó��

            InGameTCPController.Instance.SendToInGameServer(packet);

            inGameSessionInfo.isSyncOK = true;
        }
        else if((SessionInfo)realData[0] == SessionInfo.PlayerNum)
        {
            Global.Instance.StaticLog("PlayerNum Packet");
            //�÷��̾� ���°�÷��̾���
            inGameSessionInfo.playerNum = BitConverter.ToInt32(realData,1);
            inGameSessionInfo.isPlayerInfoOK = true;
            //�ε� 2�ܰ�Ϸ� ó��
        }
    }
}
