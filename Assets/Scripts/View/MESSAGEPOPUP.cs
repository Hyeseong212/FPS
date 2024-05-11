using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MESSAGEPOPUP : MonoBehaviour
{
    [SerializeField] Text messageTxt;
    [SerializeField] Button OKbtn;

    public void Start()
    {
        OKbtn.onClick.AddListener(delegate 
        {
            gameObject.SetActive(false);
        });
    }
    public void Init(int messageIdx)
    {
        string message = Global.Instance.messageInfos[messageIdx].message;
        messageTxt.text = message;
    }
}
