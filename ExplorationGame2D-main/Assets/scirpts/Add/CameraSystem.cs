using UnityEngine;
using Cinemachine;

public class CameraSystem : MonoBehaviour
{
    public CinemachineVirtualCamera camera1;
    public CinemachineVirtualCamera camera2;

    private void Update()
    {
        // 使用鼠标右键返回Camera1
        if (Input.GetMouseButtonDown(1))
        {
            SwitchCameraPriority(camera1, camera2);
        }

        // 使用鼠标左键激活Camera2，并LookAt目标
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                SwitchCameraPriority(camera2, camera1);
                camera2.LookAt = hit.transform;
                AdjustCameraLens(camera2, hit.point);
            }
        }
    }

    private void SwitchCameraPriority(CinemachineVirtualCamera highPriorityCamera, CinemachineVirtualCamera lowPriorityCamera)
    {
        highPriorityCamera.Priority = 11;  // 设置更高的优先级
        lowPriorityCamera.Priority = 10;  // 保证优先级低于另一相机
    }

    private void AdjustCameraLens(CinemachineVirtualCamera cam, Vector3 targetPoint)
    {
        // 根据需要进行调整，例如可以是固定的FieldOfView或者基于距离的动态调整
        float desiredDistance = Vector3.Distance(cam.transform.position, targetPoint);
        cam.m_Lens.FieldOfView = Mathf.Lerp(40, 80, (desiredDistance - 10) / 30);  // 数值范围和因子根据实际需要调整
    }
}
