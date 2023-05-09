using UnityEngine;
using Cinemachine;

public class GameCamera : MonoBehaviour
{
    [SerializeField] public CinemachineVirtualCamera PlayerFollowCam;
    
    private static GameCamera _instance;
    public static GameCamera instance { get { return _instance; } }

    public CameraShake Shake { get; private set; }
    
    public CinemachineVirtualCamera CurrentVCam { get; private set; }
    
    void Awake() {
        if (_instance != null && _instance != this) {
        Destroy(this.gameObject);
        }
        else {
        _instance = this;
        }

        Shake = GetComponent<CameraShake>();
    }

    void Start() {
        CurrentVCam = PlayerFollowCam;

        Shake.SetNoiseChannel( noiseChannel: CurrentVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>() );
    }
}
