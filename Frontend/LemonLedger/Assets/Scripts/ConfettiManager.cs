using UnityEngine;

public class ConfettiManager : MonoBehaviour
{
    [Tooltip("Drag your Confetti ParticleSystem prefab here")]
    public ParticleSystem confettiPrefab;

    public void Celebrate()
    {
        var ps = Instantiate(confettiPrefab, Vector3.zero, Quaternion.identity);
        ps.Play();
        Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
    }
}
