using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameMainFlow : MonoBehaviour
{
    void Awake()
    {
        InGameSessionController.Instance.Init();
    }
}
