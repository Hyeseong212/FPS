using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public Camera followedCam;
    public float speed = 5f;
    public float rotationSpeed = 10f; // 회전 속도
    public float distanceThreshold = 10f; // 거리 임계값
    private List<Node> path;
    private Vector3 finalTargetPosition;
    private int targetIndex;
    public LayerMask floorLayer; // 바닥 레이어 마스크
    public LayerMask obstacleLayer; // 장애물 레이어 마스크

    private bool isMoving = false; // 캐릭터 이동 상태

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // 오른쪽 마우스 버튼 클릭
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
                    // 임계값 이하이고 장애물이 없는 경우 직선으로 이동
                    finalTargetPosition = hit.point;
                    path = null; // A* 경로 초기화
                    isMoving = true; // 이동 시작
                }
                else
                {
                    // 임계값 초과이거나 장애물이 있는 경우 A* 알고리즘 사용
                    AStarPathfinding pathfinding = GetComponent<AStarPathfinding>();
                    Node closestNode = pathfinding.FindClosestNode(hit.point);
                    path = pathfinding.FindPath(transform.position, closestNode.Position);
                    finalTargetPosition = hit.point; // 최종 목표 위치 설정
                    targetIndex = 0;
                    isMoving = true; // 이동 시작
                    Debug.Log("Path found: " + path.Count + " nodes");
                }
            }
        }

        if (isMoving)
        {
            Vector3 targetPosition = finalTargetPosition;

            // 경로가 있는 경우 A* 알고리즘 사용
            if (path != null && targetIndex < path.Count)
            {
                targetPosition = path[targetIndex].Position;
                Debug.Log("Current Position: " + transform.position + " Target Position: " + targetPosition);
                if (Vector3.Distance(transform.position, targetPosition) < 0.1f) // 허용 오차
                {
                    targetIndex++;
                    Debug.Log("Reached target node, moving to next. Current Index: " + targetIndex);
                }
            }

            MoveTowards(targetPosition);

            if (Vector3.Distance(transform.position, finalTargetPosition) < 0.1f) // 허용 오차
            {
                isMoving = false; // 이동 완료
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
