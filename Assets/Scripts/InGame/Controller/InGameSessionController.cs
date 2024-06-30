using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InGameSessionController : MonoBehaviour
{
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
            //로딩 1단계완료 처리

            InGameTCPController.Instance.SendToInGameServer(packet);

            inGameSessionInfo.isSyncOK = true;
        }
        else if((SessionInfo)realData[0] == SessionInfo.PlayerNum)
        {
            //플레이어 몇번째플레이언지
            inGameSessionInfo.playerNum = BitConverter.ToInt32(realData,1);
            Global.Instance.StaticLog($"PlayerNum : {inGameSessionInfo.playerNum }");
            InGameTCPController.Instance.EnqueueDispatcher(() => {
                CharacterTrController.Instance.Init();
                inGameSessionInfo.isPlayerInfoOK = true;
            });
            //로딩 2단계완료 처리
        }

        //다른 플레이어 데이터
    }
}
