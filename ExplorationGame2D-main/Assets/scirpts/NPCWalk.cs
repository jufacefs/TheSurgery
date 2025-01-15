using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWalk : MonoBehaviour
{
    // Start is called before the first frame update
    
    [System.Serializable]
    public struct MovementStep
    {
        public float offsetX;
        public float offsetY;
        public float time; // 如果为0，则按默认速度移动
    }

    public MovementStep[] movementSteps; // 定义结构体数组
    public float defaultSpeed = 1f; // 默认的移动速度

    private int currentStepIndex = 0; // 当前步骤的索引
    private Vector3 startPosition;
    public bool isMoving = false;


    void Start()
    {
        StartCoroutine(MoveSprite());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator MoveSprite()
    {
        Debug.Log("movesprite is trigered");
        while (true)
        {
            if (currentStepIndex < movementSteps.Length)
            {
                MovementStep step = movementSteps[currentStepIndex];

                float targetX = transform.position.x + step.offsetX;
                float targetY = transform.position.y + step.offsetY;
                float timeToMove = step.time > 0 ? step.time : Mathf.Abs(step.offsetX) / defaultSpeed;
                if (step.offsetX == 0 && step.offsetY ==0)
                {
                    isMoving = false;
                }
                else
                {
                    isMoving = true;
                }

                Vector3 targetPosition = new Vector3(targetX, targetY, transform.position.z);

                float elapsedTime = 0f;
                Vector3 initialPosition = transform.position;

                while (elapsedTime < timeToMove)
                {
                    transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / timeToMove);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                transform.position = targetPosition; // 确保最终位置精确
                currentStepIndex++;
            }
            else
            {
                currentStepIndex = 0; 
            }

            yield return null;
        }
    }
}
