using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
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

    private List<UserEntity> JoinRequestUsers = new List<UserEntity>();

    public void Init()
    {
        Debug.Log("GuildController Init Complete");
    }
    public void ProcessGuildPacket(byte[] data)
    {
        if ((GuildProtocol)data[0] == GuildProtocol.CreateGuild)
        {

        }
        else if ((GuildProtocol)data[0] == GuildProtocol.SelectGuildCrew)
        {

        }
        else if ((GuildProtocol)data[0] == GuildProtocol.SelectGuildName)
        {
            SetGuildNameFromServer(data);
        }
        else if ((GuildProtocol)data[0] == GuildProtocol.SelectGuildUid)
        {
            byte[] guildNameByte = data.Skip(1).ToArray();
            SetGuildInfo(guildNameByte);
        }
    }
    private void SetGuildNameFromServer(byte[] data)
    {
        string strguildInfos = Encoding.UTF8.GetString(data.Skip(1).ToArray());

        List<GuildInfo> guildInfos = JsonConvert.DeserializeObject<List<GuildInfo>>(strguildInfos);
        TCPController.Instance.EnqueueDispatcher(delegate
        {
            GUILDVIEW guildView = FindObjectOfType<GUILDVIEW>(true);
            guildView.FindedGuildSort(guildInfos);
        });
    }
    private void SetGuildInfo(byte[] data)
    {
        string guildinfoSTR = Encoding.UTF8.GetString(data);

        Global.Instance.standbyInfo.guildInfo = JsonConvert.DeserializeObject<GuildInfo>(guildinfoSTR);

        TCPController.Instance.EnqueueDispatcher(delegate
        {
            GUILDVIEW guildView = FindObjectOfType<GUILDVIEW>(true);
            guildView.SetActiveGuildFindPanel(false);
            guildView.SetActiveGuildCrewsPanel(true);
            guildView.SetUserGuildName(Global.Instance.standbyInfo.guildInfo.guildName);

            guildView.JoinRequestSort();
        });
    }
}
