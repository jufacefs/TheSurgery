using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentObjectControl : MonoBehaviour
{
    public GameObject[] children;
    private Collider2D parentCollider;

    void Start()
    {
        parentCollider = GetComponent<Collider2D>();
        ValueManager.Instance.ResetValue();
        ToggleChildrenColliders(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            HandleClick(hit.collider);
        }
    }

    void HandleClick(Collider2D collider)
    {
        if (collider == parentCollider)
        {
            ValueManager.Instance.IncrementValue(); // 当点击父对象时增加数值
                                                    // 根据 currentValue 决定是否启用子对象的 Collider
            ToggleChildrenColliders(ValueManager.Instance.currentValue >= 2);
            Debug.Log("Parent clicked, children collider state updated based on currentValue.");
        }
        else
        {
            // 遍历所有子对象，查看是否点击了其中的某一个
            foreach (GameObject child in children)
            {
                Collider2D childCollider = child.GetComponent<Collider2D>();
                if (collider == childCollider && childCollider.enabled)
                {
                    Debug.Log(child.name + " child clicked.");
                    PerformChildAction(child); // 调用一个方法来处理子对象的点击
                    break; // 找到被点击的子对象后不需要继续遍历
                }
            }
        }
        // 检查是否点击了任何注册的 Collider
        bool anyColliderHit = collider != null;

        // 逻辑处理部分...

        // 如果点击的区域不是任何注册的 Collider
        if (!anyColliderHit)
        {
            ValueManager.Instance.ResetValue();
            ToggleChildrenColliders(false);
            Debug.Log("Clicked outside any recognized collider, reset currentValue and disabled all child colliders.");
        }
    }

    private void PerformChildAction(GameObject child)
    {
        // 这里可以添加与子对象相关的特定操作
        // 例如改变子对象的颜色、播放动画、启动游戏逻辑等
        child.GetComponent<SpriteRenderer>().color = Color.red; // 示例：改变颜色为红色
    }


    public void ToggleChildrenColliders(bool enable)
    {
        foreach (GameObject child in children)
        {
            Collider2D childCollider = child.GetComponent<Collider2D>();
            if (childCollider != null)
            {
                // 根据传入的 enable 参数激活或禁用 Collider
                childCollider.enabled = enable;
            }
        }
    }


}
