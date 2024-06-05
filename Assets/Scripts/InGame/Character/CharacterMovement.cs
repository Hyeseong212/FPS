using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public Camera followedCam;
    public float speed = 5f;
    public float rotationSpeed = 10f; // ȸ�� �ӵ�
    public float distanceThreshold = 10f; // �Ÿ� �Ӱ谪
    private List<Node> path;
    private Vector3 finalTargetPosition;
    private int targetIndex;
    public LayerMask floorLayer; // �ٴ� ���̾� ����ũ
    public LayerMask obstacleLayer; // ��ֹ� ���̾� ����ũ

    private bool isMoving = false; // ĳ���� �̵� ����

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // ������ ���콺 ��ư Ŭ��
        {
            Ray ray = followedCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, floorLayer))
            {
                float distance = Vector3.Distance(transform.position, hit.point);
                RaycastHit obstacleHit;
                bool hasObstacle = Physics.Raycast(transform.position, (hit.point - transform.position).normalized, out obstacleHit, distance, obstacleLayer);

                if (distance <= distanceThreshold && !hasObstacle)
                {
                    // �Ӱ谪 �����̰� ��ֹ��� ���� ��� �������� �̵�
                    finalTargetPosition = hit.point;
                    path = null; // A* ��� �ʱ�ȭ
                    isMoving = true; // �̵� ����
                }
                else
                {
                    // �Ӱ谪 �ʰ��̰ų� ��ֹ��� �ִ� ��� A* �˰��� ���
                    AStarPathfinding pathfinding = GetComponent<AStarPathfinding>();
                    Node closestNode = pathfinding.FindClosestNode(hit.point);
                    path = pathfinding.FindPath(transform.position, closestNode.Position);
                    finalTargetPosition = hit.point; // ���� ��ǥ ��ġ ����
                    targetIndex = 0;
                    isMoving = true; // �̵� ����
                    Debug.Log("Path found: " + path.Count + " nodes");
                }
            }
        }

        if (isMoving)
        {
            Vector3 targetPosition = finalTargetPosition;

            // ��ΰ� �ִ� ��� A* �˰��� ���
            if (path != null && targetIndex < path.Count)
            {
                targetPosition = path[targetIndex].Position;
                Debug.Log("Current Position: " + transform.position + " Target Position: " + targetPosition);
                if (Vector3.Distance(transform.position, targetPosition) < 0.1f) // ��� ����
                {
                    targetIndex++;
                    Debug.Log("Reached target node, moving to next. Current Index: " + targetIndex);
                }
            }

            MoveTowards(targetPosition);

            if (Vector3.Distance(transform.position, finalTargetPosition) < 0.1f) // ��� ����
            {
                isMoving = false; // �̵� �Ϸ�
                Debug.Log("Reached final target position");
            }
        }
    }

    void MoveTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }
}
