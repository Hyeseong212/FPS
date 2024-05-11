using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LOGINPOPUP : MonoBehaviour
{
    [SerializeField] Button loginBtn; 
    [SerializeField] InputField IDInputField; 
    [SerializeField] InputField PasswordInpuField;
    [SerializeField] Button signUpBtn;

    void Start()
    {
        loginBtn.onClick.AddListener(delegate
        {
            Login();
        });
        signUpBtn.onClick.AddListener(delegate
        {
            SignUp();
        });
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {

        }
    }
    private void Login() 
    {
        var message = new Packet();

        LoginInfo loginInfo = new LoginInfo();

        loginInfo.ID = IDInputField.text;

        loginInfo.PW = PasswordInpuField.text;

        string idAndPw = JsonUtility.ToJson(loginInfo);

        int length = 0x01 + 0x01 + Utils.GetLength(idAndPw);


        message.push((byte)Protocol.Login);
        message.push(length);
        message.push((byte)LoginRequestType.LoginRequest);
        message.push(idAndPw);
        TCPController.Instance.SendToServer(message);
    }
    private void SignUp()
    {
        this.gameObject.SetActive(false);
        PopupController.Instance.SetActivePopup(POPUPTYPE.SIGNUP, true);
    }
    public void LoginSuccess()
    {
        this.gameObject.SetActive(false);

    }
}
