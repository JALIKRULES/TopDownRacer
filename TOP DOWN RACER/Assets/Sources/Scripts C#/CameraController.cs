using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _cameraShakeTime;
    [SerializeField] private float _cameraShakeIntensity;

    private CinemachineVirtualCamera _cinemachineVirtualCamera;
    private CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;
    public static CameraController Instance;

    private bool _isCameraShake;
    void Start()
    {
        _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        _cinemachineBasicMultiChannelPerlin = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        Instance = this;
    }

    public void StartCameraShake()
    {
        if (_isCameraShake) return;
        StartCoroutine(CameraShake());
    }

    private IEnumerator CameraShake()
    {
        _isCameraShake = true;
        _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = _cameraShakeIntensity;
        yield return new WaitForSeconds(_cameraShakeTime);
        _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
        _isCameraShake = false;
    }


}
