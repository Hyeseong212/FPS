using UnityEngine;
public class ChatController : MonoBehaviour
{
    public void ReceiveMessage(string message)
    {
        Debug.Log(message);
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
