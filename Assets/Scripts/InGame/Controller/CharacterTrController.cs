using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterTrController : MonoBehaviour
{
    private static CharacterTrController instance;
    public static CharacterTrController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CharacterTrController>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("CharacterTrControllerSingleton");
                    instance = singletonObject.AddComponent<CharacterTrController>();
                    var singletonParent = FindObjectOfType<InGameSingleton>();
                    Instantiate(singletonObject, singletonParent.transform);
                }
            }
            return instance;
        }
    }

    //모든 유저들의 캐릭터들을 동기화시켜줌
    [SerializeField] List<GameObject> Characters = new List<GameObject>();
    public Dictionary<long, GameObject> UserTr = new Dictionary<long, GameObject>();
    GameObject ThisUserCharacter;

    public void Init()//여기엔 기본적인 Transform값만
    {
        Characters.Clear(); // 기존 캐릭터 리스트 초기화
        // Players 태그가 달린 모든 게임 오브젝트를 가져와서 Characters 리스트에 추가
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            Characters.Add(player);
        }
        UserTr.Clear();
        if (InGameSessionController.Instance.inGameSessionInfo.playerNum == -1)
        {
            return;
        }
        ThisUserCharacter = Characters[InGameSessionController.Instance.inGameSessionInfo.playerNum - 1];
        UserTr.Add(Global.Instance.standbyInfo.userEntity.UserUID, ThisUserCharacter);
        PlayerController.Instance.Init();
    }

    public void FixedUpdate()
    {
        if (InGameSessionController.Instance.inGameSessionInfo.isPlayerInfoOK && ThisUserCharacter != null)// 서버에 동기화완료되면 캐릭터 위치 뿌려줘
        {
            SendClientCharacterTr(ThisUserCharacter.transform.localPosition, ThisUserCharacter.transform.localRotation);
        }
    }

    private void SendClientCharacterTr(Vector3 clientChTr, Quaternion quaternion)
    {
        //유저 캐릭터 위치값 보내기 유저UID + 유저 캐릭터위치
        Packet packet = new Packet();

        int length = 0x01 + 0x01 + Utils.GetLength(Global.Instance.standbyInfo.userEntity.UserUID) + Utils.GetLength(clientChTr.x) + Utils.GetLength(clientChTr.y) + Utils.GetLength(clientChTr.z)
            + Utils.GetLength(quaternion.x) + Utils.GetLength(quaternion.y) + Utils.GetLength(quaternion.z) + Utils.GetLength(quaternion.w);

        packet.push((byte)InGameProtocol.CharacterTr);
        packet.push(length);
        packet.push(Global.Instance.standbyInfo.userEntity.UserUID);
        packet.push(clientChTr.x);
        packet.push(clientChTr.y);
        packet.push(clientChTr.z);
        packet.push(quaternion.x);
        packet.push(quaternion.y);
        packet.push(quaternion.z);
        packet.push(quaternion.w);

        InGameTCPController.Instance.SendToInGameServer(packet);
    }

    public void ProcessUpdatePlayerPacket(byte[] data)
    {
        if ((SessionInfo)data[0] == SessionInfo.TransformInfo)
        {
            byte[] trData = data.Skip(1).ToArray();
            UpdatePlayerTR(trData);
        }
    }
    public void UpdatePlayerTR(byte[] data)//업데이트 시켜주고
    {
        long userUID = BitConverter.ToInt64(data, 0);
        int playerNum = BitConverter.ToInt32(data, 8);
        if (!UserTr.ContainsKey(userUID))
        {
            UserTr.Add(userUID, Characters[playerNum - 1]);
        }
        else
        {
            InGameTCPController.Instance.EnqueueDispatcher(() =>
            {
                // 현재 사용자의 UID가 아니면 위치와 회전을 업데이트
                if (userUID != Global.Instance.standbyInfo.userEntity.UserUID)
                {
                    float posX = BitConverter.ToSingle(data, 12);
                    float posY = BitConverter.ToSingle(data, 16);
                    float posZ = BitConverter.ToSingle(data, 20);

                    float rotX = BitConverter.ToSingle(data, 24);
                    float rotY = BitConverter.ToSingle(data, 28);
                    float rotZ = BitConverter.ToSingle(data, 32);
                    float rotW = BitConverter.ToSingle(data, 36);

                    GameObject playerObject;
                    if (UserTr.TryGetValue(userUID, out playerObject))
                    {
                        // 해당 GameObject의 위치와 회전을 업데이트
                        playerObject.transform.localPosition = new Vector3(posX, posY, posZ);
                        playerObject.transform.localRotation = new Quaternion(rotX, rotY, rotZ, rotW);
                    }
                }
            });
        }
    }
}
