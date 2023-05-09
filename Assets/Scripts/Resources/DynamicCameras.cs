using Cinemachine;
using UnityEngine;

public class DynamicCameras : MonoBehaviour
{
    #region Variables
    private CinemachineVirtualCamera virtualCamera;
    public CamerasType cameraType;
    #endregion

    private void Start() => virtualCamera = GetComponent<CinemachineVirtualCamera>();

    public void ResetPriority() => virtualCamera.m_Priority = 0;

    public void SetPriority() => virtualCamera.m_Priority = 100;
}
