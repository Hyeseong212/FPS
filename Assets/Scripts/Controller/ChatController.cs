using UnityEngine;
public class ChatController : MonoBehaviour
{
    private static ChatController instance;
    public static ChatController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ChatController>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("ChatControllerSingleton");
                    instance = singletonObject.AddComponent<ChatController>();
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return instance;
        }
    }
    public void Init()
    {
        Debug.Log("ChatController Init Complete");
    }
    public void ReceiveMessage(string message)
    {
        //Debug.Log(message);
        TCPController.Instance.EnqueueDispatcher(() =>
        {
            // `MainView` �̱����� �ִ� ���
            MainView mainView = FindObjectOfType<MainView>();
            if (mainView != null)
            {
                mainView.chatDisplay.text += $"{message}\n"; // ���� �޽����� �Բ� �߰� ǥ��
            }
        });
    }
}
