using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;    // ī�޶� ���� ���
    public Vector3 offset;      // ī�޶�� ��� ������ ������

    void Start()
    {
        // ���� ��ġ�� ������� �ʱ� ������ ����
        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        // ����� ��ġ�� �������� ������� ī�޶� ��ġ ������Ʈ
        transform.position = target.position + offset;
    }
}
