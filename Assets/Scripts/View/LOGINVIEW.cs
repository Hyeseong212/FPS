using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;


public class LOGINVIEW : MonoBehaviour
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
        if (Input.GetKeyDown(KeyCode.F1))
        {
            IDInputField.text = "netrogold";
            PasswordInpuField.text = "Sjh011009!";
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            IDInputField.text = "Rbiotech";
            PasswordInpuField.text = "1507";
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            IDInputField.text = "netrohong";
            PasswordInpuField.text = "Sjh011009!";
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            IDInputField.text = "netrosjh";
            PasswordInpuField.text = "Sjh011009!";
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            IDInputField.text = "new";
            PasswordInpuField.text = "new!";
        }
    }
    private void Login() 
    {
        var message = new Packet();

        LoginInfo loginInfo = new LoginInfo();

        loginInfo.ID = IDInputField.text;

        loginInfo.PW = PasswordInpuField.text;

        string idAndPw = JsonConvert.SerializeObject(loginInfo);

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
        ViewController.Instance.SetActiveView(VIEWTYPE.SIGNUP, true);
    }
}
