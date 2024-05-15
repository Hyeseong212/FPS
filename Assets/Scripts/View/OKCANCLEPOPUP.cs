using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OKCANCLEPOPUP : MonoBehaviour
{
    [SerializeField] Text messageTxt;
    [SerializeField] Button OKbtn;
    [SerializeField] Button Cancelbtn;

    Action thisAction;

    public void Start()
    {
        OKbtn.onClick.AddListener(delegate
        {
            thisAction.Invoke();
            gameObject.SetActive(false);
        });
        Cancelbtn.onClick.AddListener(delegate
        {
            gameObject.SetActive(false);
        });
    }
    public void Init(int messageIdx, Action action)
    {
        thisAction = action;
        string message = Global.Instance.messageInfos[messageIdx].message;
        messageTxt.text = message;
    }
}
