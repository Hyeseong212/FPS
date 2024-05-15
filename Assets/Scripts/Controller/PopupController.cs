using System;
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

    [SerializeField] MESSAGEPOPUP messagepopup;
    [SerializeField] OKCANCLEPOPUP okcancelpopup;

    public void Init()
    {
        Debug.Log("PopupController Init Complete");
    }

    public void SetActivePopupWithMessage(POPUPTYPE type, bool isActive, int messageIdx, Action action)
    {
        switch (type)
        {
            case POPUPTYPE.MESSAGE:
                messagepopup.gameObject.SetActive(isActive);
                messagepopup.GetComponent<MESSAGEPOPUP>().Init(messageIdx);
                break;
            case POPUPTYPE.OKCANCEL:
                okcancelpopup.gameObject.SetActive(isActive);
                okcancelpopup.GetComponent<OKCANCLEPOPUP>().Init(messageIdx, action);
                break;
            default:
                Debug.Log("This Popup Type is not Exist");
                break;
        }
    }
}
