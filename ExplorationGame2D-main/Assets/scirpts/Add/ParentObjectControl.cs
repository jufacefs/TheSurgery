using UnityEngine;
using System.Collections.Generic;

public class ParentObjectControl : MonoBehaviour
{
    public GameObject itemPrefab; // 子GameObject的预制件
    public int minItems = 1; // 最少生成的项目数量
    public int maxItems = 5; // 最多生成的项目数量
    public float childSizeMultiplier = 0.1f; // 子对象大小相对于父对象的比例
    private Collider2D parentCollider;
    private float parentWidth;
    private float parentHeight;
    private bool hasGenerated = false; // 是否已经生成过子对象

    void Start()
    {
        parentCollider = GetComponent<BoxCollider2D>();
        parentWidth = parentCollider.bounds.size.x;
        parentHeight = parentCollider.bounds.size.y;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !hasGenerated)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit.collider == parentCollider)
            {
                GenerateChildrenWithinParent();
                hasGenerated = true;
            }
        }
    }

    void GenerateChildrenWithinParent()
    {
        int numberOfItems = Random.Range(minItems, maxItems + 1); // 随机生成子对象数量
        List<GameObject> children = new List<GameObject>();

        for (int i = 0; i < numberOfItems; i++)
        {
            // 使用嵌套类生成子类
            ChildObjectControl childControl = new ChildObjectControl();
            SpriteRenderer parentRenderer = GetComponent<SpriteRenderer>(); // 获取父对象的SpriteRenderer组件
            int parentSortingOrder = parentRenderer != null ? parentRenderer.sortingOrder : 0;
            GameObject child = childControl.GenerateChild(itemPrefab, transform, parentWidth, parentHeight, childSizeMultiplier, children, parentSortingOrder);

            SetChildRenderingOrder(child);  // 设置渲染顺序
            children.Add(child);
        }
    }

    void SetChildRenderingOrder(GameObject child)
    {
        SpriteRenderer childRenderer = child.GetComponent<SpriteRenderer>();
        if (childRenderer != null)
        {
            childRenderer.sortingLayerName = "Foreground"; // 假设"Foreground"在父对象的层之上
            childRenderer.sortingOrder = 1; // 确保此值大于父对象的sortingOrder
        }
    }

    // 嵌套类用于生成子类
    private class ChildObjectControl
    {
        public GameObject GenerateChild(GameObject itemPrefab, Transform parent, float parentWidth, float parentHeight, float childSizeMultiplier, List<GameObject> existingChildren, int parentSortingOrder = 0)
        {
            GameObject child = Instantiate(itemPrefab, parent);
            float scaledChildWidth = parentWidth * childSizeMultiplier;
            float scaledChildHeight = parentHeight * childSizeMultiplier;
            child.transform.localScale = new Vector3(scaledChildWidth, scaledChildHeight, 1);

            // 调整渲染排序层级，确保子对象位于父对象之上
            SpriteRenderer childRenderer = child.GetComponent<SpriteRenderer>();
            if (childRenderer != null)
            {
                childRenderer.sortingOrder = parentSortingOrder + 1;
            }

            // 确保collider立即更新其边界
            //child.GetComponent<BoxCollider2D>().size = new Vector2(scaledChildWidth, scaledChildHeight);
            bool isPlaced = PositionChild(child, scaledChildWidth, scaledChildHeight, parentWidth, parentHeight, existingChildren);

            if (!isPlaced)
            {
                Destroy(child); // 如果放置失败，则销毁该子对象
            }
            else
            {
                existingChildren.Add(child); // 成功放置后添加到列表
            }

            return child;
        }

        private bool PositionChild(GameObject child, float width, float height, float parentWidth, float parentHeight, List<GameObject> existingChildren)
        {
            BoxCollider2D childCollider = child.GetComponent<BoxCollider2D>();
            if (childCollider == null)
            {
                Debug.LogError("Child object is missing BoxCollider2D component!");
                return false;
            }

            bool placed = false;
            int attempts = 0;
            while (!placed && attempts < 100)
            {
                float minX = -parentWidth / 2 + width / 2;
                float maxX = parentWidth / 2 - width / 2;
                float minY = -parentHeight / 2 + height / 2;
                float maxY = parentHeight / 2 - height / 2;

                float posX = Random.Range(minX, maxX);
                float posY = Random.Range(minY, maxY);
                Vector3 potentialPosition = new Vector3(posX, posY, -1);

                childCollider.enabled = false; // 禁用collider进行重叠检测
                child.transform.localPosition = potentialPosition;
                childCollider.enabled = true; // 重新启用collider

                if (!IsColliding(childCollider, existingChildren))
                {
                    placed = true;
                }

                attempts++;
            }

            if (!placed)
            {
                Debug.LogError("Failed to place child object without collision!");
            }

            return placed;
        }

        private bool IsColliding(BoxCollider2D childCollider, List<GameObject> existingChildren)
        {
            foreach (GameObject existingChild in existingChildren)
            {
                BoxCollider2D existingCollider = existingChild.GetComponent<BoxCollider2D>();
                if (existingCollider != null && childCollider.bounds.Intersects(existingCollider.bounds))
                {
                    return true;
                }
            }
            return false;
        }
    }

}
