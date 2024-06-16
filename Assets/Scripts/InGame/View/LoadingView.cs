using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingView : MonoBehaviour
{
    [SerializeField] Button testbtn;
    private void Start()
    {
        testbtn.onClick.AddListener(delegate
        {
            this.gameObject.SetActive(false);
            //FindObjectOfType<CharacterTrController>().TestBool = true;
        });
    }
}
