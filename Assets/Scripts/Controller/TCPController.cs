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
                }
            }
            return instance;
        }
    }


    ChatController chatController;

    public void StartTCPController()
    {
        ConnectToServer("127.0.0.1", 9000); // ���� IP �ּ� �� ��Ʈ
        chatController = new ChatController();
    }

    void OnApplicationQuit()
    {
        // ���ø����̼� ���� �� Ŭ���̾�Ʈ ����
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

        try
        {
            while (true)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead > 0)
                {
                    HandlePacket(buffer); // ���ŵ� �޽����� �м��ϴ� �Լ� ȣ��
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Receive error: {e.Message}");
        }
    }
    private void HandlePacket(byte[] buffer)
    {
        var protocol = buffer[0];

        var lengthbytes = new byte[4];

        for(int i = 0; i < 4; i++)
        {
            lengthbytes[i] = buffer[i + 1];
        }

        int length = BitConverter.ToInt32(lengthbytes, 0);

        //count = count - 1;
        //byte[] realdata = new byte[count];
        //for (int i = count-1; i > 0; i--)
        //{
        //    realdata[i] = buffer[i];
        //}

        switch (protocol)
        {
            case (byte)Protocol.Chat:
                string message = Encoding.UTF8.GetString(buffer, 5, length);
                string str = "";
                for (int i = 5; i < length + 5; i++)
                {
                    if (i != length + 5 - 1) str += buffer[i] + "|";
                    else str += buffer[i];
                }
                Debug.Log("length : " + length + ", data : " + str);
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
    //private void HandlePacket(string message)
    //{
    //    if (message.StartsWith("Server:"))
    //    {
    //        string content = message.Substring(7).Trim(); // "Server:" ���λ縦 �����Ͽ� ���� ������ ����
    //        // ���ŵ� ��Ŷ�� ���뿡 ���� ó�� ���� ����
    //        switch (content)
    //        {
    //            case string s when s.Contains("Hello"):
    //                Debug.Log("Received a greeting from the server.");
    //                chatController.ReceiveMessage($"Server: Hello!");
    //                break;

    //            case string s when s.Contains("time"):
    //                Debug.Log($"Received the current server time: {content}");
    //                chatController.ReceiveMessage(content);
    //                break;

    //            default:
    //                Debug.Log($"Received an unknown message from the server: {content}");
    //                chatController.ReceiveMessage($"Unknown message from the server: {content}");
    //                break;
    //        }
    //    }
    //    else
    //    {
    //        Debug.Log($"Received unrecognized packet: {message}");
    //    }
    //}

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

    // ���� �����忡�� �۾��� �����ϴ� �޼���
    public void EnqueueDispatcher(Action action)
    {
        lock (ExecutionQueue)
        {
            ExecutionQueue.Enqueue(action);
        }
    }

    // �� �����Ӹ��� ť�� �ִ� ��� �۾��� �����ϴ� �޼���
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