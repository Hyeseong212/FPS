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
    public void Init()
    {
        standbyInfo = new StandbyInfo();
        Debug.Log("Global Init Complete");
    }
}
