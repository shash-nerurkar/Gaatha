using UnityEngine;
using Cinemachine;
using System.Runtime.InteropServices;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private NoiseSettings defaultNoiseProfile;

    private Timer shakeTimer;

    private float currentShakeIntensity = 0;

    private CinemachineBasicMultiChannelPerlin noiseChannel;

    void Start() {
        shakeTimer = gameObject.AddComponent<Timer>();
    }

    public void SetNoiseChannel(CinemachineBasicMultiChannelPerlin noiseChannel) => this.noiseChannel = noiseChannel;

    // SHAKE THE CAMERA
    public void ShakeCamera( float intensity, float time, [ Optional ] NoiseSettings customNoiseProfile ) {
        // SET NOISE PROFILE TO DEFAULT, IF NONE IS SPECIFIED
        if ( customNoiseProfile == null )
            noiseChannel.m_NoiseProfile = defaultNoiseProfile;
        else
            noiseChannel.m_NoiseProfile = customNoiseProfile;

        currentShakeIntensity = intensity;

        // TO INTERPOLATE THE INTENSITY OF THE SHAKE OVER TIME
        shakeTimer.StartTimer( maxTime: time, onTimerFinish: onShakeFinished );
    }

    void onShakeFinished() {}

    // INTERPOLATING THE SHAKE INTENSITY
    void Update() {
        noiseChannel.m_AmplitudeGain = Mathf.Lerp( 0 , currentShakeIntensity , shakeTimer.TimeRemaining );
    }
}
