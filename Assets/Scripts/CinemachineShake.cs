using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake Instance {  get; private set; }

    private CinemachineCamera cinemachineCamera;
    private CinemachineBasicMultiChannelPerlin noise;

    private float shakeTimer;
    private float shakeTimerTotal;
    private float startingIntensity;

    [SerializeField] private float intensity;
    [SerializeField] private float time;

    private void Awake()
    {
        Instance = this;

        cinemachineCamera = GetComponent<CinemachineCamera>();
        noise = GetComponent<CinemachineBasicMultiChannelPerlin>();

        if (noise == null)
        {
            Debug.LogWarning("CinemachineBasicMultiChannelPerlin component missing!");
        }

    }

    public void ShakeCamera()
    {
        if (noise == null) return;

        noise.AmplitudeGain = intensity;
        noise.FrequencyGain= intensity;
        startingIntensity = intensity;
        shakeTimer = time;
        shakeTimerTotal = time;
    }

    private void Update()
    {
        if (noise == null) return;

        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            noise.AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, 1f - (shakeTimer / shakeTimerTotal));
            noise.FrequencyGain = Mathf.Lerp(startingIntensity, 0f, 1f - (shakeTimer / shakeTimerTotal));
        }
    }
}
