using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUILDVIEW : MonoBehaviour
{
    [Header("헤더")]
    [SerializeField] Text guildNameTxt;
    [Header("길드 찾기 패널")]
    [SerializeField] GameObject guildFindPanel;
    [SerializeField] InputField findGuildNameTextPanel;
    [SerializeField] GameObject guildNameContainer;
    [SerializeField] GameObject guildNameObject;

    [SerializeField] Button GuildFindBtn;

    [SerializeField] InputField createGuildName;
    [SerializeField] Button GuildCreate;

    [Header("길드원 패널")]
    [SerializeField] GameObject guildCrewsPanel;
    [SerializeField] GameObject guildCrewsContainer;
    [SerializeField] GameObject guildCrewsNameObject;

    [SerializeField] GameObject joinRequestContainerObject;
    [SerializeField] GameObject joinRequestBtn;

    [SerializeField] Button guildResignBtn;

    [Header("풋")]
    [SerializeField] Button GuildFindPanelBtn;
    [SerializeField] Button GuildCrewPanelBtn;

    List<GameObject> guildInfoObject = new List<GameObject>();
    List<GameObject> guildCrewObject = new List<GameObject>();
    List<GameObject> JoinRequestObject = new List<GameObject>();

    //테스트 코드
    public List<GuildInfo> guildInfos = new List<GuildInfo>();
    public List<GuildCrew> guildcrews = new List<GuildCrew>();

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
        guildResignBtn.onClick.AddListener(delegate
        {
            GuildResign();
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
        findGuildNameTextPanel.text = "";

        for (int i = 0; i < guildInfoObject.Count; i++)
        {
            Destroy(guildInfoObject[i]);
        }
        guildInfoObject.Clear();

        guildFindPanel.SetActive(false);
        guildCrewsPanel.SetActive(false);

        if (Global.Instance.standbyInfo.userEntity.guildUID != 0)//길드가입되어있을경우
        {
            Packet sendingGuildInfoRequest = new Packet();

            int length = 0x01 + Utils.GetLength(Global.Instance.standbyInfo.userEntity.guildUID);

            sendingGuildInfoRequest.push((byte)Protocol.Guild);
            sendingGuildInfoRequest.push(length);
            sendingGuildInfoRequest.push((byte)GuildProtocol.SelectGuildUid);
            sendingGuildInfoRequest.push(Global.Instance.standbyInfo.userEntity.guildUID);

            TCPController.Instance.SendToServer(sendingGuildInfoRequest);

            createGuildName.gameObject.SetActive(false);
            GuildCreate.gameObject.SetActive(false);

            guildResignBtn.gameObject.SetActive(true);
        }
        else//가입안되어있을경우
        {
            SetUserGuildName("가입된 길드가 없습니다");



            createGuildName.gameObject.SetActive(true);
            GuildCreate.gameObject.SetActive(true);

            guildResignBtn.gameObject.SetActive(false);
        }
    }
    public void GuildResingUpdate()
    {
        SetUserGuildName("가입된 길드가 없습니다");

        createGuildName.gameObject.SetActive(true);
        GuildCreate.gameObject.SetActive(true);

        guildResignBtn.gameObject.SetActive(false);

        var gcc = guildCrewsContainer.GetComponentsInChildren<Button>();

        for (int i = 0; i < gcc.Length; i++)
        {
            Destroy(gcc[i].gameObject);
        }
        guildCrewObject.Clear();
    }
    private void GuildResign()
    {
        Packet packet = new Packet();

        int length = 0x01 + Utils.GetLength(Global.Instance.standbyInfo.userEntity.UserUID);

        packet.push((byte)Protocol.Guild);
        packet.push(length);
        packet.push((byte)GuildProtocol.GuildResign);
        packet.push(Global.Instance.standbyInfo.userEntity.UserUID);

        TCPController.Instance.SendToServer(packet);
    }
    private void FindGuild()
    {
        for (int i = 0; i < guildInfoObject.Count; i++)
        {
            Destroy(guildInfoObject[i]);
        }
        guildInfoObject.Clear();

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

    public void UpdateGuildInfo(GuildInfo guildinfo)
    {
        //크루패널에 크루추가
        for (int i = 0; i < guildCrewObject.Count; i++)
        {
            Destroy(guildCrewObject[i]);
        }

        for (int i = 0; i < guildinfo.guildCrews.Count; i++)
        {
            GameObject go = Instantiate(guildCrewsNameObject, guildCrewsContainer.transform);
            go.SetActive(true);
            guildCrewObject.Add(go);
            go.AddComponent<GuildCrewInfo>();
            go.GetComponent<GuildCrewInfo>().guildCrew = guildinfo.guildCrews[i];
            go.GetComponentInChildren<Text>().text = guildinfo.guildCrews[i].crewName;
        }
        JoinRequestSort();
    }
    public void FindedGuildSort(List<GuildInfo> guildInfos)
    {
        for (int i = 0; i < guildInfos.Count; i++)
        {
            GameObject GuildNameObject = Instantiate(guildNameObject, guildNameContainer.transform);
            guildInfoObject.Add(GuildNameObject);
            GuildNameObject.SetActive(true);
            GuildNameObject.GetComponentInChildren<Text>().text = guildInfos[i].guildName;
            GuildNameObject.GetComponent<GuildProfile>().guildinfo = guildInfos[i];
            GuildNameObject.GetComponent<Button>().onClick.AddListener(delegate
            {
                if (Global.Instance.standbyInfo.userEntity.guildUID != 0)//길드가 가입돼있을경우
                {
                    PopupController.Instance.SetActivePopupWithMessage(POPUPTYPE.MESSAGE, true, 3, null, null);
                }
                else//가입안돼있을경우
                {
                    //길드 가입 요청 보내기
                    Action action = () =>
                    {
                        Packet packet = new Packet();

                        int length = 0x01 + Utils.GetLength(Global.Instance.standbyInfo.userEntity.UserUID) + Utils.GetLength(GuildNameObject.GetComponent<GuildProfile>().guildinfo.guildUid);

                        packet.push((byte)Protocol.Guild);
                        packet.push(length);
                        packet.push((byte)GuildProtocol.RequestJoinGuild);
                        packet.push(Global.Instance.standbyInfo.userEntity.UserUID);
                        packet.push(GuildNameObject.GetComponent<GuildProfile>().guildinfo.guildUid);
                        TCPController.Instance.SendToServer(packet);
                    };
                    PopupController.Instance.SetActivePopupWithMessage(POPUPTYPE.OKCANCEL, true, 2, action,null);
                }
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
                //귓속말 보내기 플로팅 띄우기
                Debug.Log($"this is {GuildNameObject.GetComponentInChildren<Text>().text}");
            });
        }
    }


    public void JoinRequestSort()
    {
        for (int i = 0; i < JoinRequestObject.Count; i++)
        {
            Destroy(JoinRequestObject[i]);
        }

        for (int i = 0; i < Global.Instance.standbyInfo.guildInfo.guildRequest.Count; i++)
        {
            GameObject GuildNameObject = Instantiate(joinRequestBtn, joinRequestContainerObject.transform);
            JoinRequestObject.Add(GuildNameObject);
            GuildNameObject.SetActive(true);
            GuildNameObject.AddComponent<JoinRequestInfo>();
            GuildNameObject.GetComponent<JoinRequestInfo>().RequestUser = Global.Instance.standbyInfo.guildInfo.guildRequest[i];
            GuildNameObject.GetComponentInChildren<Text>().text = Global.Instance.standbyInfo.guildInfo.guildRequest[i].UserName;
            GuildNameObject.GetComponent<Button>().onClick.AddListener(delegate
            {
                Action action = () =>
                {
                    Destroy(GuildNameObject);

                    Packet sendServerRequestOK = new Packet();

                    int length = 0x01 + Utils.GetLength(GuildNameObject.GetComponent<JoinRequestInfo>().RequestUser.UserUID) + Utils.GetLength(Global.Instance.standbyInfo.userEntity.guildUID);

                    sendServerRequestOK.push((byte)Protocol.Guild);
                    sendServerRequestOK.push(length);
                    sendServerRequestOK.push((byte)GuildProtocol.RequestJoinOK);
                    sendServerRequestOK.push(GuildNameObject.GetComponent<JoinRequestInfo>().RequestUser.UserUID);
                    sendServerRequestOK.push(Global.Instance.standbyInfo.userEntity.guildUID);

                    TCPController.Instance.SendToServer(sendServerRequestOK);
                    //Debug.Log($"Sending Server To Request User {GuildNameObject.GetComponent<JoinRequestInfo>().RequestUser.UserUID} OK");
                };
                PopupController.Instance.SetActivePopupWithMessage(POPUPTYPE.OKCANCEL, true, 4, action,null);
                //요청 팝업 띄우기
                Debug.Log($"this is {GuildNameObject.GetComponentInChildren<Text>().text}");
            });
        }
    }
    public void GuildCreatePakcetToServer()
    {
        Packet packet = new Packet();

        int length = 0x01 + Utils.GetLength(Global.Instance.standbyInfo.userEntity.UserUID) + Utils.GetLength(createGuildName.text);

        packet.push((byte)Protocol.Guild);
        packet.push(length);
        packet.push((byte)GuildProtocol.CreateGuild);
        packet.push(Global.Instance.standbyInfo.userEntity.UserUID);
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
