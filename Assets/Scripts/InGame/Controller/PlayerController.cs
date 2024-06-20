using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerController instance;
    public static PlayerController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerController>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("PlayerControllerSingleton");
                    instance = singletonObject.AddComponent<PlayerController>();
                    var singletonParent = FindObjectOfType<InGameSingleton>();
                    Instantiate(singletonObject, singletonParent.transform);
                }
            }
            return instance;
        }
    }
    [SerializeField] GameObject[] players;
    public void Init()
    {
        Debug.Log("PlayerController Init Complete");
        var characterCam = FindObjectOfType<CameraFollow>();
        switch (InGameSessionController.Instance.inGameSessionInfo.playerNum)
        {
            case 1:
                characterCam.target = players[0].transform;

                break;
            case 2:
                characterCam.target = players[1].transform;
                break;
            case 3:
                characterCam.target = players[2].transform;
                break;
            case 4:
                characterCam.target = players[3].transform;
                break;
        }
        for (int i = 0; i < players.Length; i++)
        {
            if (InGameSessionController.Instance.inGameSessionInfo.playerNum != i + 1)
            {
                players[i].GetComponent<CharacterMovement>().enabled = false;
                players[i].GetComponent<AStarPathfinding>().enabled = false;
            }

        }
    }


}
