using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class TCPController : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;
    private Thread receiveThread;
    private const int bufferSize = 409600;

    private static TCPController instance;
    public static TCPController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TCPController>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("TCPControllerSingleton");
                    instance = singletonObject.AddComponent<TCPController>();
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return instance;
        }
    }


    ChatController chatController;

    public void Init()
    {
        Debug.Log("TCPController Init Complete");
        ConnectToServer(Global.Instance.companyip, 9000); // 서버 IP 주소 및 포트
        chatController = new ChatController();
    }

    void OnApplicationQuit()
    {
        // 애플리케이션 종료 시 클라이언트 종료
        Disconnect();
    }

    public void ConnectToServer(string serverIp, int serverPort)
    {
        try
        {
            client = new TcpClient(serverIp, serverPort);
            stream = client.GetStream();
            receiveThread = new Thread(new ThreadStart(ReceivePackets));
            receiveThread.Start();
            Debug.Log("Connected to server.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Connection error: {e.Message}");
        }
    }

    public void Disconnect()
    {
        if (stream != null) stream.Close();
        if (client != null) client.Close();
        if (receiveThread != null) receiveThread.Abort();
        Debug.Log("Disconnected from server.");
    }

    private void ReceivePackets()
    {
        byte[] buffer = new byte[Packet.buffersize];

        //try
        //{
            while (true)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead > 0)
                {
                    HandlePacket(buffer); // 수신된 메시지를 분석하는 함수 호출
                }
            }
        //}
        //catch (Exception e)
        //{
        //    Debug.LogError($"Receive error: {e.Message}");
        //}
    }
    private void HandlePacket(byte[] buffer)
    {
        byte protocol = buffer[0];
        byte[] lengthBytes = new byte[4];

        try
        {
            for (int i = 0; i < 4; i++)
            {
                lengthBytes[i] = buffer[i + 1];
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        int length = BitConverter.ToInt32(lengthBytes, 0);
        byte[] realData = new byte[length];

        try
        {
            for (int i = 0; i < length; i++)
            {
                realData[i] = buffer[i + 5];
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
        switch (protocol)
        {
            case (byte)Protocol.Login:
                if ((LoginRequestType)realData[0] == LoginRequestType.LoginRequest)
                {
                    if ((ResponseType)realData[1] == ResponseType.Success)
                    {
                        long sendUseruidval = BitConverter.ToInt64(realData, 2);
                        Global.Instance.standbyInfo.userUid = sendUseruidval;
                        EnqueueDispatcher(() =>
                        {
                            Debug.Log("로긴 성공");
                            PopupController.Instance.SetActivePopup(POPUPTYPE.LOGIN, false);
                        });
                    }
                    else if ((ResponseType)realData[1] == ResponseType.Fail)
                    {
                        Debug.Log("응 로그인 실패 ㅅㄱ");
                    }
                }
                else if ((LoginRequestType)realData[0] == LoginRequestType.LogoutRequest)
                {
                    if ((ResponseType)realData[1] == ResponseType.Success)
                    {
                        //Debug.Log(message);
                        EnqueueDispatcher(() =>
                        {
                            PopupController.Instance.SetActivePopup(POPUPTYPE.LOGIN, true);
                            Debug.Log("로그아웃 성공");
                        });
                    }
                    else if ((ResponseType)realData[1] == ResponseType.Fail)
                    {
                        Debug.Log("응 로그아웃 실패 ㅅㄱ");
                    }
                }
                else if((LoginRequestType)realData[0] == LoginRequestType.SignupRequest)
                {
                    if ((ResponseType)realData[1] == ResponseType.Success)
                    {
                        //Debug.Log(message);
                        EnqueueDispatcher(() =>
                        {
                            PopupController.Instance.SetActivePopup(POPUPTYPE.SIGNUP, false);
                            PopupController.Instance.SetActivePopup(POPUPTYPE.LOGIN, true);
                            PopupController.Instance.SetActivePopupWithMessage(POPUPTYPE.MESSAGE, true, 1);
                            Debug.Log("회원가입 성공");
                        });
                    }
                    else if ((ResponseType)realData[1] == ResponseType.Fail)
                    {
                        EnqueueDispatcher(() =>
                        {
                            PopupController.Instance.SetActivePopupWithMessage(POPUPTYPE.MESSAGE, true, 0);
                            Debug.Log("응 회원가입 실패 ㅅㄱ");
                        });
                    }
                }

                break;
            case (byte)Protocol.Chat:
                string message = Encoding.UTF8.GetString(buffer, 5, length);
                string str = "";
                for (int i = 5; i < length + 5; i++)
                {
                    if (i != length + 5 - 1) str += buffer[i] + "|";
                    else str += buffer[i];
                }
                //Debug.Log("length : " + length + ", data : " + str);
                chatController.ReceiveMessage(message);
                break;
            case (byte)Protocol.Match:
                break;

            default:
                //Debug.Log("Something Come in");
                //string str = "";
                //for (int i = 0; i < buffer.Length; i++)
                //{
                //    if (i != buffer.Length - 1)
                //        str += buffer[i].ToString() + "|";
                //    else
                //        str += buffer[i].ToString();
                //}
                //Debug.Log(str);
                break;
        }
    }


    public void SendToServer(Packet data)
    {
        try
        {
            if (stream != null)
            {
                stream.Write(data.buffer, 0, data.position);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Send error: {e.Message}");
        }
    }

    private static readonly Queue<Action> ExecutionQueue = new Queue<Action>();

    // 메인 스레드에서 작업을 예약하는 메서드
    public void EnqueueDispatcher(Action action)
    {
        lock (ExecutionQueue)
        {
            ExecutionQueue.Enqueue(action);
        }
    }

    // 매 프레임마다 큐에 있는 모든 작업을 실행하는 메서드
    public void ExecutePending()
    {
        lock (ExecutionQueue)
        {
            while (ExecutionQueue.Count > 0)
            {
                ExecutionQueue.Dequeue().Invoke();
            }
        }
    }

    private void Update()
    {
        ExecutePending();
    }
}