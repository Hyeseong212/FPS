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

    [SerializeField] LOGINPOPUP loginpopup;
    [SerializeField] SIGNUPPOPUP signpopup;
    [SerializeField] MESSAGEPOPUP messagepopup;

    public void Init()
    {
        Debug.Log("PopupController Init Complete");
    }
    public void SetActivePopup(POPUPTYPE type, bool isActive)
    {
        switch (type)
        {
            case POPUPTYPE.LOGIN:
                loginpopup.gameObject.SetActive(isActive);
                break;
            case POPUPTYPE.SIGNUP:
                signpopup.gameObject.SetActive(isActive);
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
