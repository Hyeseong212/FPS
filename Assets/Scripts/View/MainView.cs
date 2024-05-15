using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class MainView : MonoBehaviour
{
    public long ReceiveUID = 638506276349467624;


    public InputField inputField;
    
    public Text chatDisplay;
    public Dropdown dropdown;

    private StreamWriter writer;

    [SerializeField] Button sendBtn;
    [SerializeField] Button logoutBtn;
    [SerializeField] Button guildOpenBtn;

    ChatStatus ChatStatus = ChatStatus.ENTIRE;

    private void Start()
    {
        sendBtn.onClick.AddListener(delegate
        {
            SendMessage();
        });
        guildOpenBtn.onClick.AddListener(delegate
        {
            ViewController.Instance.SetActiveView(VIEWTYPE.GUILD, true);
        });
        dropdown.onValueChanged.AddListener(delegate
        {
            switch (dropdown.value)
            {
                case 0:
                    ChatStatus = ChatStatus.ENTIRE;
                    break;
                case 1:
                    ChatStatus = ChatStatus.WHISPER;
                    break;
                case 2:
                    ChatStatus = ChatStatus.GUILD;
                    break;
            }
            
        });
        logoutBtn.onClick.AddListener(delegate
        {
            Logout();
        });
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            SendMessage();
    }
    private void Logout()
    {
        var message = new Packet();

        int length = 0x01 + Utils.GetLength(Global.Instance.standbyInfo.userUid);

        message.push((byte)Protocol.Login);
        message.push(length);
        message.push((byte)LoginRequestType.LogoutRequest);
        message.push(Global.Instance.standbyInfo.userUid);
        TCPController.Instance.SendToServer(message);
    }
    public void SendMessage()
    {
        if(ChatStatus == ChatStatus.ENTIRE)
        {
            var message = new Packet();

            int length = 0x01 + Utils.GetLength(Global.Instance.standbyInfo.userUid) + Utils.GetLength(inputField.text);

            message.push((byte)Protocol.Chat);
            message.push(length);
            message.push((byte)ChatStatus);
            message.push(Global.Instance.standbyInfo.userUid);
            message.push(inputField.text);
            TCPController.Instance.SendToServer(message);
            inputField.text = "";
        }
        else if (ChatStatus == ChatStatus.WHISPER)
        {
            //이제 여기에서 보내는 유저 이름을 알려줘야됨
            var message = new Packet();

            int length = 0x01 + Utils.GetLength(Global.Instance.standbyInfo.userUid) + Utils.GetLength(ReceiveUID) + Utils.GetLength(inputField.text);

            message.push((byte)Protocol.Chat);
            message.push(length);
            message.push((byte)ChatStatus);
            message.push(Global.Instance.standbyInfo.userUid);
            message.push(ReceiveUID);
            message.push(inputField.text);
            TCPController.Instance.SendToServer(message);
            inputField.text = "";
        }
        else if (ChatStatus == ChatStatus.GUILD)
        {
       
        }

    }

}
