using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingView : MonoBehaviour
{
    [SerializeField] Button SendTestPacket;

    void Start()
    {
        SendTestPacket.onClick.AddListener(delegate 
        {
        });
    }

    // Update is called once per frame

}
