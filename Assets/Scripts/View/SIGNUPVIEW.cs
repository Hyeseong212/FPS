using UnityEngine;
using Newtonsoft.Json;

using UnityEngine.UI;

public class SIGNUPVIEW : MonoBehaviour
{
    [SerializeField] InputField nameInputfield;
    [SerializeField] InputField idInputfield;
    [SerializeField] InputField pwInputfield;
    [SerializeField] Button signUpBtn;
    [SerializeField] Button BackBtn;
    private void Start()
    {
        signUpBtn.onClick.AddListener(delegate 
        {
            SignUp();
        });
        BackBtn.onClick.AddListener(delegate
        {
            gameObject.SetActive(false);
            ViewController.Instance.SetActiveView(VIEWTYPE.LOGIN, true);
        });
    }
    private void SignUp()
    {
        SignUpInfo signUpInfo = new SignUpInfo();

        signUpInfo.name = nameInputfield.text; 
        signUpInfo.id = idInputfield.text; 
        signUpInfo.pw = pwInputfield.text;

        string signupInfoJSON = JsonConvert.SerializeObject(signUpInfo);
        int Length = 0x01 + Utils.GetLength(signupInfoJSON);

        Packet packet = new Packet();
        packet.push((byte)Protocol.Login);
        packet.push(Length);
        packet.push((byte)LoginRequestType.SignupRequest);
        packet.push(signupInfoJSON);
        TCPController.Instance.SendToServer(packet);
    }
}
