using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class MainView : MonoBehaviour
{
    public long UID = 638506276349467625;
    public long sendingUID = 6385062763494676243;

    public InputField inputField;
    public Text chatDisplay;
    public Dropdown dropdown;

    private StreamWriter writer;

    [SerializeField] Button sendBtn;

    ChatStatus ChatStatus = ChatStatus.ENTIRE;

    private void Start()
    {
        Login();
        sendBtn.onClick.AddListener(delegate
        {
            SendMessage();
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
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            SendMessage();
    }
    public void Login()
    {
        var message = new Packet();

        int length = 0x01 + Utils.GetLength(UID);

        message.push((byte)Protocol.Login);
        message.push(length);
        message.push(UID);
        TCPController.Instance.SendToServer(message);
    }
    public void SendMessage()
    {
        if(ChatStatus == ChatStatus.ENTIRE)
        {
            var message = new Packet();

            int length = 0x01 + Utils.GetLength(UID) + Utils.GetLength(inputField.text);

            message.push((byte)Protocol.Chat);
            message.push(length);
            message.push((byte)ChatStatus);
            message.push(UID);
            message.push(inputField.text);
            TCPController.Instance.SendToServer(message);
            inputField.text = "";
        }
        else if (ChatStatus == ChatStatus.WHISPER)
        {
            //이제 여기에서 보내는 유저 이름을 알려줘야됨
            var message = new Packet();

            int length = 0x01 + Utils.GetLength(UID) + Utils.GetLength(sendingUID) + Utils.GetLength(inputField.text);

            message.push((byte)Protocol.Chat);
            message.push(length);
            message.push((byte)ChatStatus);
            message.push(UID);
            message.push(sendingUID);
            message.push(inputField.text);
            TCPController.Instance.SendToServer(message);
            inputField.text = "";
        }
        else if (ChatStatus == ChatStatus.GUILD)
        {
       
        }

    }

}
