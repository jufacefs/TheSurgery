using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandboxController : MonoBehaviour
{
    public GameObject parentPrefab; // 父类预制体
    public GameObject childPrefab; // 子类预制体

    private void Update()
    {
        // 检测鼠标左键点击
        if (Input.GetMouseButtonDown(0))
        {
            // 发射一条射线检测鼠标位置是否在父类对象上
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // 如果点击到父类对象，则生成子类对象
                if (hit.collider.gameObject.CompareTag("ParentObject"))
                {
                    InstantiateChildObject(hit.collider.transform.position);
                }
            }
        }
    }

    private void InstantiateChildObject(Vector3 position)
    {
        // 实例化子类对象并设置位置
        GameObject childObject = Instantiate(childPrefab, position, Quaternion.identity);
        // 给子类对象添加Collider组件
        childObject.AddComponent<BoxCollider>();
    }

    // 在Unity编辑器中可以通过拖拽方式设置父类和子类预制体
}
