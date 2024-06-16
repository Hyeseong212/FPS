using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTrController : MonoBehaviour
{
    //��� �������� ĳ���͵��� ����ȭ������
    public List<(float, Vector3)> usersTr;
    public GameObject UserCharacter;
    public bool TestBool = false;




    public void FixedUpdate()
    {
        if (TestBool)
            SendClientCharacterTr(UserCharacter.transform.localPosition, UserCharacter.transform.localRotation);
    }

    private void SendClientCharacterTr(Vector3 clientChTr, Quaternion quaternion)
    {
        //���� ĳ���� ��ġ�� ������ ����UID + ���� ĳ������ġ
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

    }
}
