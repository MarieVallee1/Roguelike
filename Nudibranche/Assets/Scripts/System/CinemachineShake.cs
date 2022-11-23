using Cinemachine;
using UnityEngine;

namespace System
{
    public class CinemachineShake : MonoBehaviour
    {
        public static CinemachineShake instance;
        
        private CinemachineVirtualCamera _cinemachineCam;
        private CinemachineBasicMultiChannelPerlin _cinemachineBasic;
        private float _shakeTimer;
    
        void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this);
            }
            instance = this;
            
            _cinemachineCam = GetComponent<CinemachineVirtualCamera>();
        }

        public void ShakeCamera(float intensity, float time)
        {
            _cinemachineBasic = _cinemachineCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            _cinemachineBasic.m_AmplitudeGain = intensity;
            _shakeTimer = time;
        }

        private void Update()
        {
            if (_shakeTimer > 0)
            {
                _shakeTimer -= Time.deltaTime;
                if (_shakeTimer <= 0f)
                {
                   _cinemachineBasic = _cinemachineCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                   _cinemachineBasic.m_AmplitudeGain = 0f;
                }
            }
        }
    }
}
