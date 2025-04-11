using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("--- Camera Shake Settings ---")]
    [SerializeField][HideInInspector] private CinemachineImpulseSource impulseSource;

    void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void Shake(float intensity)
    {
        impulseSource.GenerateImpulse(intensity);
    }
}