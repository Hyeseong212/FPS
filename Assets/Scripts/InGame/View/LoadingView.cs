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
            Packet packet = new Packet();
            packet.push("something");
            TCPController.Instance.SendToServer(packet);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
