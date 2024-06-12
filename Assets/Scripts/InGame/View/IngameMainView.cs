using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameMainView : MonoBehaviour
{
    [SerializeField] Button TestBtn;

    [SerializeField] GameObject characterTr;

    bool isSendingTestPacket = false;

    void Start()
    {
        TestBtn.onClick.AddListener(delegate
        {
            isSendingTestPacket = !isSendingTestPacket;
        });
    }
    void FixedUpdate()
    {
        if (isSendingTestPacket)
        {
            var testObject = FindAnyObjectByType<CharacterTrController>();
            testObject.SendClientCharacterTr(characterTr.transform.localPosition, characterTr.transform.localRotation);
        }
    }
}
