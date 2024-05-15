using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUILDVIEW : MonoBehaviour
{
    [Header("���")]
    [SerializeField] Text guildNameTxt;
    [Header("��� ã�� �г�")]
    [SerializeField] GameObject guildFindPanel;
    [SerializeField] InputField findGuildNameTextPanel;
    [SerializeField] GameObject guildNameContainer;
    [SerializeField] GameObject guildNameObject;

    [SerializeField] Button GuildFindBtn;

    [SerializeField] InputField createGuildName;
    [SerializeField] Button GuildCreate;

    [Header("���� �г�")]
    [SerializeField] GameObject guildCrewsPanel;
    [SerializeField] GameObject guildCrewsContainer;
    [SerializeField] GameObject guildCrewsNameObject;


    [Header("ǲ")]
    [SerializeField] Button GuildFindPanelBtn;
    [SerializeField] Button GuildCrewPanelBtn;

    List<GameObject> guildInfoObject = new List<GameObject>();

    //�׽�Ʈ �ڵ�
    public List<GuildInfo> guildInfos = new List<GuildInfo>();
    List<GuildCrew> guildcrews = new List<GuildCrew>();

    private void Start()
    {
        GuildFindBtn.onClick.AddListener(delegate
        {
            FindGuild();
        });
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
    private void FindGuild()
    {
        for(int i = 0; i < guildInfoObject.Count; i++)
        {
            Destroy(guildInfoObject[i]);
        }

        Packet packet = new Packet();

        int length = 0x01 + Utils.GetLength(findGuildNameTextPanel.text);

        packet.push((byte)Protocol.Guild);
        packet.push(length);
        packet.push((byte)GuildProtocol.SelectGuildName);
        packet.push(findGuildNameTextPanel.text);

        TCPController.Instance.SendToServer(packet);
    }
    public void TestCode2()
    {
        GuildCrewsNameSort(guildcrews);
    }
    public void FindedGuildSort(List<GuildInfo> guildInfos)
    {
        for (int i = 0; i < guildInfos.Count; i++)
        {
            GameObject GuildNameObject = Instantiate(guildNameObject, guildNameContainer.transform);
            guildInfoObject.Add(GuildNameObject);
            GuildNameObject.SetActive(true);
            GuildNameObject.GetComponentInChildren<Text>().text = guildInfos[i].guildName;
            GuildNameObject.GetComponent<Button>().onClick.AddListener(delegate 
            {

                //��� ���� ��û ������
                Action action = () => 
                {
                    Packet packet = new Packet();

                    int length = 0x01 + Utils.GetLength(Global.Instance.standbyInfo.userUid);

                    packet.push((byte)Protocol.Guild);
                    packet.push(length);
                    packet.push((byte)GuildProtocol.RequestJoinGuild);
                    packet.push(Global.Instance.standbyInfo.userUid);

                    Debug.Log("�������� ��û����");
                };
                PopupController.Instance.SetActivePopupWithMessage(POPUPTYPE.OKCANCEL, true, 2,action);
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
