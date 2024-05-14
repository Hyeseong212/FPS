using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupController : MonoBehaviour
{
    private static PopupController instance;
    public static PopupController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PopupController>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("PopupControllerSingletone");
                    instance = singletonObject.AddComponent<PopupController>();
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return instance;
        }
    }

    [SerializeField] LOGINVIEW loginpopup;
    [SerializeField] SIGNUPVIEW signpopup;
    [SerializeField] GUILDVIEW guildView;
    [SerializeField] MESSAGEPOPUP messagepopup;

    public void Init()
    {
        Debug.Log("PopupController Init Complete");
    }
    public void SetActiveView(VIEWTYPE type, bool isActive)
    {
        switch (type)
        {
            case VIEWTYPE.LOGIN:
                loginpopup.gameObject.SetActive(isActive);
                break;
            case VIEWTYPE.SIGNUP:
                signpopup.gameObject.SetActive(isActive);
                break;
            case VIEWTYPE.GUILD:
                guildView.gameObject.SetActive(isActive);
                break;
            default:
                Debug.Log("This Popup Type is not Exist");
                break;
        }
    }

    public void SetActivePopupWithMessage(POPUPTYPE type, bool isActive, int messageIdx)
    {
        switch (type)
        {
            case POPUPTYPE.MESSAGE:
                messagepopup.gameObject.SetActive(isActive);
                messagepopup.GetComponent<MESSAGEPOPUP>().Init(messageIdx);
                break;
            default:
                Debug.Log("This Popup Type is not Exist");
                break;
        }
    }
}
