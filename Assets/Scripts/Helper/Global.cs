using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{

    private static Global instance;
    public static Global Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Global>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("GlobalSingleton");
                    instance = singletonObject.AddComponent<Global>();
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return instance;
        }
    }
    public StandbyInfo standbyInfo;
    public List<MessageInfo> messageInfos;
    public string companyip = "192.168.123.1";
    public string homeip = "192.168.219.100";
    public void Init()
    {
        standbyInfo = new StandbyInfo();
        Debug.Log("Global Init Complete");
        messageInfos = new List<MessageInfo>()
        {
            new MessageInfo() {  idx = 0, message = "중복된 ID입니다 다른아이디를 입력해주세요." },
            new MessageInfo() {  idx = 1, message = "회원가입 성공" }
        };
    }
}
