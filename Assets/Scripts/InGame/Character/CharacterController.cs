using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // ���� ���콺 ��ư Ŭ�� ����
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }
    }
}
