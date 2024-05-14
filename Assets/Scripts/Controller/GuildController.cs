using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GuildController : MonoBehaviour
{
    private static GuildController instance;
    public static GuildController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GuildController>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("GuildControllerSingleton");
                    instance = singletonObject.AddComponent<GuildController>();
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return instance;
        }
    }



    public void Init()
    {
        Debug.Log("GuildController Init Complete");
    }
    public void ProcessGuildPacket(byte[] data)
    {
        if ((GuildProtocol)data[0] == GuildProtocol.CreateGuild)
        {

        }
        else if ((GuildProtocol)data[0] == GuildProtocol.IsUserGuildEnable)
        {
            SendPacketGetGuildName(data);
        }
        else if ((GuildProtocol)data[0] == GuildProtocol.SelectGuildCrew)
        {

        }
        else if ((GuildProtocol)data[0] == GuildProtocol.SelectGuildName)
        {

        }
        else if ((GuildProtocol)data[0] == GuildProtocol.SelectGuildUid)
        {
            byte[] guildNameByte = data.Skip(1).ToArray();
            SetGuildName(guildNameByte);
        }
    }
    private void SendPacketGetGuildName(byte[] data)
    {
        long uidval = BitConverter.ToInt64(data.Skip(1).ToArray(), 0);
        Packet packet = new Packet();
        if (uidval != long.MinValue)
        {
            int length = 0x01 + Utils.GetLength(uidval);

            packet.push((byte)Protocol.Guild);
            packet.push(length);
            packet.push((byte)GuildProtocol.SelectGuildUid);
            packet.push(uidval);

            TCPController.Instance.SendToServer(packet);
        }
        else
        {
            TCPController.Instance.EnqueueDispatcher(delegate
            {
                GUILDVIEW guildView = FindObjectOfType<GUILDVIEW>(true);
                guildView.SetActiveGuildFindPanel(true);
                guildView.SetUserGuildName("가입된 길드가 없습니다");
            });
        }


    }
    private void SetGuildName(byte[] data)
    {
        string guildName = Encoding.UTF8.GetString(data);

        TCPController.Instance.EnqueueDispatcher(delegate
        {
            GUILDVIEW guildView = FindObjectOfType<GUILDVIEW>(true);
            guildView.SetActiveGuildFindPanel(false);
            guildView.SetActiveGuildCrewsPanel(true);
            guildView.SetUserGuildName(guildName);
        });
    }
}
