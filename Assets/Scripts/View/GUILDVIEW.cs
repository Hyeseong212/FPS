using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUILDVIEW : MonoBehaviour
{
    [SerializeField] Text guildNameTxt;

    [SerializeField] GameObject guildFindPanel;
    [SerializeField] InputField guildNameTextPanel;
    [SerializeField] GameObject guildNameContainer;
    [SerializeField] GameObject guildNameObject;
    [SerializeField] InputField createGuildName;


    [SerializeField] GameObject guildCrewsPanel;
    [SerializeField] GameObject guildCrewsContainer;
    [SerializeField] GameObject guildCrewsNameObject;

    [SerializeField] Button GuildCreate;

    [SerializeField] Button GuildFindPanelBtn;
    [SerializeField] Button GuildCrewPanelBtn;



    public List<GuildInfo> guildInfos = new List<GuildInfo>();
    List<GuildCrew> guildcrews = new List<GuildCrew>();

    private void Start()
    {
        GuildCreate.onClick.AddListener(delegate
        {
            GuildCreatePakcetToServer();
        });
        GuildFindPanelBtn.onClick.AddListener(delegate
        {
            guildCrewsPanel.SetActive(false);
            guildFindPanel.SetActive(true);
        });
        GuildCrewPanelBtn.onClick.AddListener(delegate
        {
            guildCrewsPanel.SetActive(true);
            guildFindPanel.SetActive(false);
        });
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            guildFindPanel.SetActive(false);
            gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        //���⼭ �������ٰ� ���� ������ ��忡 ���ԵǾ��ִ��� Ȯ���ؾߵ�
        int length = 0x01 + Utils.GetLength(Global.Instance.standbyInfo.userUid);

        Packet packet = new Packet();
        packet.push((byte)Protocol.Guild);
        packet.push(length);
        packet.push((byte)GuildProtocol.IsUserGuildEnable);
        packet.push(Global.Instance.standbyInfo.userUid);

        TCPController.Instance.SendToServer(packet);
    }
    public void TestCode()
    {
        FindedGuildSort(guildInfos);
    }
    public void TestCode2()
    {
        GuildCrewsNameSort(guildcrews);
    }
    private void FindedGuildSort(List<GuildInfo> guildInfos)
    {
        for (int i = 0; i < guildInfos.Count; i++)
        {
            GameObject GuildNameObject = Instantiate(guildNameObject, guildNameContainer.transform);
            GuildNameObject.SetActive(true);
            GuildNameObject.GetComponentInChildren<Text>().text = guildInfos[i].guildName;
            GuildNameObject.GetComponent<Button>().onClick.AddListener(delegate 
            {
                //��� ���� ��û ������
                Debug.Log($"this is {GuildNameObject.GetComponentInChildren<Text>().text}");
            });
        }
    }
    private void GuildCrewsNameSort(List<GuildCrew> guildCrews)
    {
        for (int i = 0; i < guildInfos.Count; i++)
        {
            GameObject GuildNameObject = Instantiate(guildCrewsNameObject, guildCrewsContainer.transform);
            GuildNameObject.SetActive(true);
            GuildNameObject.GetComponentInChildren<Text>().text = guildCrews[i].crewName;
            GuildNameObject.GetComponent<Button>().onClick.AddListener(delegate
            {
                //�ӼӸ� ������ �÷��� ����
                Debug.Log($"this is {GuildNameObject.GetComponentInChildren<Text>().text}");
            });
        }
    }
    public void GuildCreatePakcetToServer()
    {
        Packet packet = new Packet();

        int length = 0x01 + Utils.GetLength(Global.Instance.standbyInfo.userUid) + Utils.GetLength(createGuildName.text);

        packet.push((byte)Protocol.Guild);
        packet.push(length);
        packet.push((byte)GuildProtocol.CreateGuild);
        packet.push(Global.Instance.standbyInfo.userUid);
        packet.push(createGuildName.text);

        TCPController.Instance.SendToServer(packet);

        createGuildName.text = "";
    }


    public void SetActiveGuildFindPanel(bool isActive)
    {
        guildFindPanel.SetActive(isActive);
    }
    public void SetActiveGuildCrewsPanel(bool isActive)
    {
        guildCrewsPanel.SetActive(isActive);
    }
    public void SetUserGuildName(string guildName)
    {
        guildNameTxt.text = guildName;
    }
}
