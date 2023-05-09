using System.Collections;
using System.Linq;
using Cinemachine;
using Player;
using UnityEngine;

namespace Manager
{
    public class CameraManager : MonoBehaviour
    {
        #region Variables
        private DynamicCameras[] dynamicCameras;
        private DynamicCameras activeCam, lastActiveCam;
        private CinemachineBrain brain;
        private Coroutine shakeRoutine;
        private float lastY;
        private bool isUpdateOnY;
        private Coroutine updateOnYRoutine, stopOnYRoutine;
        private float m_Tx = -.2f, m_Ty = 3.5f;
        #endregion

        public DynamicCameras GetActiveCam => activeCam;

        private void Awake()
        {
            Init();
        }

        private void OnEnable()
        {
            EventManager.StartListening(EventKeys.OnGameStarted, _ =>
            {
                SetPriorityTo(CamerasType.Follow_CAM);
            });

            EventManager.StartListening(EventKeys.FinishTriggered, _ =>
            {
                SetPriorityTo(CamerasType.Finish_CAM);
            });
        }

        private void OnDisable()
        {
            EventManager.StopListening(EventKeys.OnGameStarted, _ =>
            {
                SetPriorityTo(CamerasType.Follow_CAM);
            });

            EventManager.StopListening(EventKeys.FinishTriggered, _ =>
            {
                SetPriorityTo(CamerasType.Finish_CAM);
            });
        }

        public void Init()
        {
            dynamicCameras = FindObjectsOfType<DynamicCameras>();

            activeCam = GetCamera(CamerasType.Start_CAM);

            Camera mainCamera = Camera.main;
            foreach (DynamicCameras dynamicCamera in dynamicCameras)
            {
                var mCamera = dynamicCamera.GetComponent<CinemachineVirtualCamera>();
                if (mainCamera != null) AdjustCamera(mainCamera.aspect, mCamera);

                if (mCamera.m_Priority > activeCam.GetComponent<CinemachineVirtualCamera>().m_Priority)
                {
                    activeCam = dynamicCamera;
                }
            }

            brain = GetComponent<CinemachineBrain>();
            Vector3 m_TrackedObjectOffset = GetActiveCam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset;
            m_Tx = m_TrackedObjectOffset.x;
            m_Ty = m_TrackedObjectOffset.y;
        }

        private void SetPriorityTo(CamerasType cameraType)
        {
            lastActiveCam = activeCam;
            activeCam.ResetPriority();
            activeCam = GetCamera(cameraType);
            activeCam.SetPriority();
        }

        private void BackLastActiveCam()
        {
            if (!lastActiveCam)
            {
                SetPriorityTo(0);
                return;
            }

            SetPriorityTo(lastActiveCam.cameraType);
        }

        private DynamicCameras GetCamera(CamerasType cameraType)
        {
            return dynamicCameras.FirstOrDefault(dynamicCamera => dynamicCamera.cameraType == cameraType);
        }

        private void ChangeCameraUpdateType(CinemachineBrain.UpdateMethod updateMethod)
        {
            GetComponent<CinemachineBrain>().m_UpdateMethod = updateMethod;
        }

        private void ShakeCamera(float duration, float amplitude, float frequency)
        {
            if (shakeRoutine != null)
            {
                StopCoroutine(shakeRoutine);
            }

            CinemachineBasicMultiChannelPerlin perlin;
            for (int i = 0; i < dynamicCameras.Length; i++)
            {
                perlin = dynamicCameras[i].GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                perlin.m_AmplitudeGain = amplitude;
                perlin.m_FrequencyGain = frequency;
            }

            shakeRoutine = StartCoroutine(ResetPerlinNoise(duration));
        }

        private IEnumerator ResetPerlinNoise(float duration)
        {
            yield return BetterWaitForSeconds.Wait(duration);

            CinemachineBasicMultiChannelPerlin perlin;
            for (int i = 0; i < dynamicCameras.Length; i++)
            {
                perlin = dynamicCameras[i].GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                perlin.m_AmplitudeGain = 0;
                perlin.m_FrequencyGain = 0;
            }
            shakeRoutine = null;
        }

        private bool CameraChangeIsCompleted()
        {
            if (!brain.IsBlending) { return true; }

            return brain.ActiveBlend.IsComplete;
        }

        private void AdjustCamera(float aspect, CinemachineVirtualCamera mCamera)
        {
            var overAspect = 1f / aspect;
            var mOrthographicSize = mCamera.m_Lens.OrthographicSize;
            //m_camera.m_Lens.FieldOfView = 2f * Mathf.Atan(Mathf.Tan(m_fieldOfView * Mathf.Deg2Rad * 0.5f) * overAspect) * Mathf.Rad2Deg;
            mCamera.m_Lens.OrthographicSize = mOrthographicSize * overAspect;
        }
    }
}