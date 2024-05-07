using UnityEngine;
public class ChatController : MonoBehaviour
{
    public void ReceiveMessage(string message)
    {
        Debug.Log(message);
        TCPController.Instance.EnqueueDispatcher(() =>
        {
            // `MainView` 싱글톤이 있는 경우
            MainView mainView = FindObjectOfType<MainView>();
            if (mainView != null)
            {
                mainView.chatDisplay.text += $"{message}\n"; // 이전 메시지와 함께 추가 표시
            }
        });
    }
}
