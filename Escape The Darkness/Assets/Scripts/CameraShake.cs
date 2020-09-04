using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    #region Private Fields
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    private float shakeTimer;

    private float startingIntesity;
    private float shakeTimeTotal;
    #endregion

    private void Awake()
    {
        Instance = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float _intensity, float _time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = _intensity;
        shakeTimer = _time;
        startingIntesity = _intensity;
        shakeTimeTotal = _time;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;

            if (shakeTimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                    cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;//snappy and better for explosions
                //cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 
                //    Mathf.Lerp(startingIntesity, 0f, shakeTimer / shakeTimeTotal);//smoother and better for subtle shake
            }
        }      
    }
}
