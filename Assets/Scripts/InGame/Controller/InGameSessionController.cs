using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InGameSessionController : MonoBehaviour
{
    //여기서 세션정보들 다처리해버리자 머리아파
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
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return instance;
        }
    }

    public InGameSessionInfo inGameSessionInfo;

    public void Init()
    {
        Global.Instance.StaticLog($"{this.ToString()} Init Complete");
    }

    public void ProcessSessionPacket(byte[] realData)
    {
        if((SessionInfo)realData[0] == SessionInfo.SessionSyncOK)
        {
            Packet packet = new Packet();

            int length = 0x01 + Utils.GetLength(Global.Instance.standbyInfo.userEntity.UserUID);

            packet.push((byte)InGameProtocol.SessionInfo);
            packet.push(length);
            packet.push((byte)SessionInfo.PlayerNum);
            packet.push(Global.Instance.standbyInfo.userEntity.UserUID);
            //로딩 1단계완료 처리

            InGameTCPController.Instance.SendToInGameServer(packet);
        }
        else if((SessionInfo)realData[0] == SessionInfo.PlayerNum)
        {
            //플레이어 몇번째플레이언지
            inGameSessionInfo.PlayerNum = BitConverter.ToInt32(realData,1);
            //로딩 2단계완료 처리
        }
    }
}
